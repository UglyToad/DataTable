namespace UglyToad.DataTable.ConsoleRunner
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Reflection;
    using Enums;
    using Tests.Unit.Factories;

    public class Program
    {
        static void Main(string[] args)
        {
            DataTableConverter dtp = new DataTableConverter();

            int count = 2000000;

            List<TestClass> classes = new List<TestClass>();

            for (int i = 0; i < count; i++)
            {
                classes.Add(new TestClass
                    {
                        Id = i,
                        Name = "Name" + i,
                        Postcode = "GU" + i,
                        CreationDate = new DateTime(2001, 1, 1),
                        ModifiedDate = new DateTime(2001, 1, 1)
                    });
            }

            DataTable dt = DataTableFactory.GenerateDataTableFilledWithObjects(classes);

            
            dtp.DataTableParserSettings.Resolver = Resolver.Default;

            Stopwatch sw = Stopwatch.StartNew();
            dtp.ConvertToObjectList<TestClass>(dt);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds + " Milliseconds Total");
            Console.WriteLine(sw.ElapsedMilliseconds/(decimal)count + " Milliseconds Per Object");
            
            if (Debugger.IsAttached) Debugger.Break();
        }
    }

    internal class TestClass
    {
        public string Name { get; set; }

        public DateTime CreationDate { get; set; }

        public int Id { get; set; }

        public string Postcode { get; set; }

        public DateTime ModifiedDate { get; set; }
    }

    public class Tony
    {
        public string Name { get; set; }
    }

    public class InitializeTonies
    {   
        public void NormalSetter(int n)
        {
            for (int i = 0; i < n; i++)
            {
                Tony t = new Tony();

                t.Name = "Tony" + i;
            }
        }

        public void ReflectionSetter(int n)
        {
            PropertyInfo propertyInfo = typeof(Tony).GetProperties()[0];

            for (int i = 0; i < n; i++)
            {
                Tony t = new Tony();
                propertyInfo.SetValue(t, "Tony" + i);
            }
        }

        public void DelegateSetter(int n)
        {
            MethodInfo methodInfo = typeof(Tony).GetProperties()[0].GetSetMethod();

            Action<Tony, string> setterDelegate = (Action<Tony, string>)Delegate.CreateDelegate(
                type: typeof(Action<Tony, string>),
                method: methodInfo);

            for (int i = 0; i < n; i++)
            {
                Tony t = new Tony();

                setterDelegate(t, "Tony" + i);
            }
        }

        public void DelegateExpressionSetter(int n)
        {
            MethodInfo methodInfo = typeof(Tony).GetProperties()[0].GetSetMethod();

            Action<Tony, object> setterDelegate = SetterDelegateUtil.GenerateSetterForMethod<Tony>(methodInfo);

            for (int i = 0; i < n; i++)
            {
                Tony t = new Tony();

                setterDelegate(t, "Tony" + i);
            }
        }
    }

    public class SetterDelegateUtil
    {
        public static Action<T, object> GenerateSetterForMethod<T>(MethodInfo methodInfo)
        {
            MethodInfo genericMethod = typeof(SetterDelegateUtil).GetMethod("GenerateSetterForMethodHelper");

            MethodInfo constructedHelper = genericMethod.MakeGenericMethod(
                typeof(T),
                methodInfo.GetParameters()[0].ParameterType);

            object returnAction = constructedHelper.Invoke(obj: null, parameters: new object[] {methodInfo});

            return (Action<T, object>)returnAction;
        }

        public static Action<TTarget, object> GenerateSetterForMethodHelper<TTarget, TParam>(MethodInfo methodInfo)
        {
            Action<TTarget, TParam> action = 
                (Action<TTarget, TParam>)Delegate.CreateDelegate(
                typeof(Action<TTarget, TParam>), methodInfo);

            Action<TTarget, object> returnAction = (TTarget target, object param) => action(target, (TParam)param);

            return returnAction;
        }
    }
}

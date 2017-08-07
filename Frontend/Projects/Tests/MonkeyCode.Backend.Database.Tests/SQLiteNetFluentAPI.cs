using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using NUnit.Framework;
using SQLite.Net;


namespace MonkeyCode.Backend.Database.Tests
{

    [TestFixture]
    [Ignore("Avoiding use of SQliteNet's attributes is not portable, therefore skip this test. Because the solution is kinda tricky (and works on Windows) I like to check it in...")]
    public class SQLiteNetFluentAPITest
    {
        public interface IEntity
        {
            void SetPropertyAttribute<T>(string propertyNameToChange, IEnumerable attributeParameters);
        }

        public abstract class Entity : IEntity
        {
            public void SetPropertyAttribute<T>(string propertyNameToChange, IEnumerable attributeParameters)
            {


            }

        }

        public class SomeClass : Entity
        {
            public SomeClass()
            {
                
            }

            
            public int OldPrimaryKey { get; set; }
            public string NewPrimaryKey { get; set; }
            public string Test { get; set; }
        }


        [TestCase]
        public void TestFluentApi()
        {
            /*
             * NOT PORTABLE! IOS PREVENTS DYNAMIC CODE GENERATION!
             * https://developer.xamarin.com/guides/ios/advanced_topics/limitations/
             */


            SomeClass tmp = new SomeClass();

            DisplayPropertyInfo(tmp);

            object newTmp = SQLiteNetFluentAPI.SetPropertyAttribute<SQLite.Net.Attributes.PrimaryKeyAttribute>(tmp, "NewPrimaryKey",null);
            newTmp = SQLiteNetFluentAPI.SetPropertyAttribute<SQLite.Net.Attributes.NotNullAttribute>(newTmp, "OldPrimaryKey", null);
            newTmp = SQLiteNetFluentAPI.SetPropertyAttribute<SQLite.Net.Attributes.MaxLengthAttribute>(newTmp, "Test", new[] {1});


            DisplayPropertyInfo(newTmp);

            SQLiteNetFluentAPI.SetProperty(newTmp, "NewPrimaryKey", "TestValue");
            SQLiteNetFluentAPI.SetProperty(newTmp, "OldPrimaryKey", 1);
            System.Console.WriteLine(SQLiteNetFluentAPI.GetProperty<string>(newTmp, "NewPrimaryKey"));

            
            string testDb1 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Test.db";
            string testDb2 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Test_new.db";

            File.Delete(testDb1);
            File.Delete(testDb2);


            var db = new SQLiteConnection(new SQLite.Net.Platform.Win32.SQLitePlatformWin32(), testDb1);
            db.CreateTable(tmp.GetType());

            var db2 = new SQLiteConnection(new SQLite.Net.Platform.Win32.SQLitePlatformWin32(), testDb2);
            db2.CreateTable(newTmp.GetType());

        }


        private static void DisplayPropertyInfo(object obj)
        {
            Type t = obj.GetType();

            Debug.WriteLine(t.GetProperties().Count());

            foreach (PropertyInfo p in t.GetProperties())
            {
                Debug.WriteLine("Fullname: " + t.FullName);

                foreach (Attribute a in p.GetCustomAttributes(typeof(Attribute), true))
                {
                    Debug.WriteLine("Attribute: " + ((MemberInfo)a.TypeId).Name);
                }
            }
        }
    }

    public class SQLiteNetFluentAPI
    {
        private static void CreatePropertyAttribute(PropertyBuilder propertyBuilder, Type attributeType, IEnumerable parameterValues)
        {
            parameterValues = parameterValues ?? new ArrayList();
            IEnumerable enumerable = parameterValues as object[] ?? parameterValues.Cast<object>().ToArray();
            var parameterTypes = (from object t in enumerable select t.GetType()).ToArray();
            ConstructorInfo propertyAttributeInfo = attributeType.GetConstructor(parameterTypes);
            if (propertyAttributeInfo != null)
            {
                var customAttributeBuilder = new CustomAttributeBuilder(propertyAttributeInfo, enumerable.Cast<object>().ToArray());
                propertyBuilder.SetCustomAttribute(customAttributeBuilder);
            }
        }

        private static PropertyBuilder CreateAutomaticProperty(TypeBuilder typeBuilder, PropertyInfo propertyInfo)
        {
            string propertyName = propertyInfo.Name;
            Type propertyType = propertyInfo.PropertyType;

            // Generate a private field
            FieldBuilder field = typeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            // Generate a public property
            PropertyBuilder property = typeBuilder.DefineProperty(propertyName, PropertyAttributes.None, propertyType, null);

            // The property set and property get methods require a special set of attributes:
            const MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName;

            // Define the "get" accessor method for current private field.
            MethodBuilder currGetPropMthdBldr = typeBuilder.DefineMethod("get_" + propertyName, getSetAttr, propertyType, Type.EmptyTypes);

            // Intermediate Language stuff...
            ILGenerator currGetIl = currGetPropMthdBldr.GetILGenerator();
            currGetIl.Emit(OpCodes.Ldarg_0);
            currGetIl.Emit(OpCodes.Ldfld, field);
            currGetIl.Emit(OpCodes.Ret);

            // Define the "set" accessor method for current private field.
            MethodBuilder currSetPropMthdBldr = typeBuilder.DefineMethod("set_" + propertyName, getSetAttr, null, new[] {propertyType});

            // Again some Intermediate Language stuff...
            ILGenerator currSetIl = currSetPropMthdBldr.GetILGenerator();
            currSetIl.Emit(OpCodes.Ldarg_0);
            currSetIl.Emit(OpCodes.Ldarg_1);
            currSetIl.Emit(OpCodes.Stfld, field);
            currSetIl.Emit(OpCodes.Ret);

            // Last, we must map the two methods created above to our PropertyBuilder to 
            // their corresponding behaviors, "get" and "set" respectively. 
            property.SetGetMethod(currGetPropMthdBldr);
            property.SetSetMethod(currSetPropMthdBldr);

            return property;

        }

        public static object SetPropertyAttribute<T>(object obj, string propertyNameToChange, IEnumerable attributeParameters ) where T : Attribute
        {
            if(!(typeof(Attribute).IsAssignableFrom(typeof(T))))
                throw new NotSupportedException();

            // Create the typeBuilder
            AssemblyName assembly = new AssemblyName("MonkeyCode.Framework.Dynamic");
            AppDomain appDomain = System.Threading.Thread.GetDomain();
            AssemblyBuilder assemblyBuilder = appDomain.DefineDynamicAssembly(assembly, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assembly.Name);

            // Create the class
            Type objType = obj.GetType();
            TypeBuilder typeBuilder = moduleBuilder.DefineType(objType.Name,
                TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.AnsiClass |
                TypeAttributes.BeforeFieldInit, typeof (System.Object));

            
            foreach (var propertyInfo in objType.GetProperties())
            {

                string propertyName = propertyInfo.Name;
                Type propertyType = propertyInfo.PropertyType;

                // Create an automatic property
                PropertyBuilder propertyBuilder = CreateAutomaticProperty(typeBuilder, propertyInfo);


                foreach (Attribute a in propertyInfo.GetCustomAttributes(typeof(Attribute), true))
                {
                    //ToDo Copy attribute values
                    CreatePropertyAttribute(propertyBuilder, a.GetType(), null);
                }

                if (propertyNameToChange != propertyName)
                    continue;

                // Set new attribute
                CreatePropertyAttribute(propertyBuilder, typeof(T), attributeParameters);

            }

            // Generate our type
            Type generatedType = typeBuilder.CreateType();

            // Now we have our type. Let's create an instance from it:
            object generatedObject = Activator.CreateInstance(generatedType);

            return generatedObject;
        }


        public static void SetProperty<T>(object o, string propertyName, T value)
        {
            Invoke<object>(o,"set_" + propertyName,new object[] {value});
        }

        public static T GetProperty<T>(object o, string propertyName)
        {
            return Invoke<T>(o, "get_" + propertyName, new object[] {});
        }


        public static T Invoke<T>(object o, string methodName, object[] parameter)
        {
            MethodInfo method = o.GetType().GetMethod(methodName);
            if (null == method)
                throw new NotSupportedException();

            return (T)(method.Invoke(o, parameter));
        }

        public static T InvokeGeneric<T>(object o, string methodName, object[] parameter,params Type[] typeArguments)
        {
            MethodInfo method = o.GetType().GetMethod(methodName);
            MethodInfo generic = method.MakeGenericMethod(typeArguments);

            var returnValue = generic.Invoke(o, parameter);

            return (T)Convert.ChangeType(returnValue, typeof(T));
        }

    }

}


   


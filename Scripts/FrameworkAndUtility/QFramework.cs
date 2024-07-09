
using System.Collections.Generic;
using System;
using UnityEngine;
namespace QFramework
{
    #region Architecture
    public interface IArchitecture
    {
        void RegisterSystem<T>(T system) where T : ISystem;
        void RegisterModel<T>(T model) where T : IModel;
        void RegisterUtiliity<T>(T utility) where T : IUtility;
        T GetModel<T>() where T : class, IModel;
        T GetSystem<T>() where T : class, ISystem;
        T GetUtility<T>() where T : class, IUtility;
        void SendCommand<T>() where T : ICommand, new();
        void SendCommand<T>(T command) where T : ICommand;
        TResult SendQuery<TResult>(IQuery<TResult> query);
        void SendEvent<T>() where T : new();
        void SendEvent<T>(T e);
        IUnRegister RegisterEvent<T>(Action<T> onEvent);
        void UnRegisterEvent<T>(Action<T> onEvent);
    }
    /// <summary>
    /// 结构
    /// 类似单例模式 仅在内部访问
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()
    {
        private static T mArchitecture;
        /// <summary>
        /// 是否初始化完成
        /// </summary>
        private bool mInited = false;
        private List<IModel> mModel = new List<IModel>();
        private List<ISystem> mSystem = new List<ISystem>();
        public static Action<T> OnRegisterPatch = architecture => { };
        public static IArchitecture Interface
        {
            get
            {
                if (mArchitecture == null)
                {
                    MakeSureArchitecture();
                }
                return mArchitecture;
            }
        }
        /// <summary>
        /// 确保container是有实例的
        /// </summary>
        static void MakeSureArchitecture()
        {
            if (mArchitecture == null)
            {
                mArchitecture = new T();
                mArchitecture.Init();
                OnRegisterPatch?.Invoke(mArchitecture);
                foreach (var architectureModel in mArchitecture.mModel)
                {
                    architectureModel.Init();
                }
                mArchitecture.mModel.Clear();
                foreach (var architectureSystem in mArchitecture.mSystem)
                {
                    architectureSystem.Init();
                }
                mArchitecture.mSystem.Clear();
                mArchitecture.mInited = true;
            }
        }
        /// <summary>
        /// 留给子类注册模块
        /// </summary>
        protected abstract void Init();
        private IOCContainer container = new IOCContainer();
        /// <summary>
        /// 提供一个获取模块的APi
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>() where T : class
        {
            MakeSureArchitecture();
            return mArchitecture.container.Get<T>();
        }
        /// <summary>
        /// 提供一个注册模块的API
        /// </summary>
        public static void Register<T>(T instance)
        {
            MakeSureArchitecture();
            mArchitecture.container.register<T>(instance);
        }
        /// <summary>
        /// 提供注册Model的api
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        public void RegisterModel<TModel>(TModel model) where TModel : IModel
        {
            model.SetArchitecture(this);
            container.register<TModel>(model);
            if (!mInited)
                mModel.Add(model);
            else
                model.Init();
        }
        public Utility GetUtility<Utility>() where Utility : class, IUtility
        {
            return container.Get<Utility>();
        }

        public void RegisterUtiliity<TUtility>(TUtility utility) where TUtility : IUtility
        {
            container.register<TUtility>(utility);
        }

        public void RegisterSystem<TSystem>(TSystem system) where TSystem : ISystem
        {
            system.SetArchitecture(this);
            container.register<TSystem>(system);
            if (!mInited)
                mSystem.Add(system);
            else
                system.Init();
        }

        public TModel GetModel<TModel>() where TModel : class, IModel
        {
            return container.Get<TModel>();
        }
        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return container.Get<TSystem>();
        }

        public void SendCommand<TCommand>() where TCommand : ICommand, new()
        {
            var command = new TCommand();
            command.SetArchitecture(this);
            command.Execute();
        }

        public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            command.SetArchitecture(this);
            command.Execute();
        }
        private ITypeEventSystem mTypeEventSystem = new TypeEventSystem();
        public void SendEvent<TEvent>() where TEvent : new()
        {
            mTypeEventSystem.Send<TEvent>();
        }

        public void SendEvent<TEvent>(TEvent e)
        {
            mTypeEventSystem.Send<TEvent>(e);
        }

        public IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            return mTypeEventSystem.Register<TEvent>(onEvent);
        }

        public void UnRegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            mTypeEventSystem.UnRegister<TEvent>(onEvent);
        }

        public TResult SendQuery<TResult>(IQuery<TResult> query)
        {
            query.SetArchitecture(this);
            return query.Do();
        }
    }
    #endregion
    #region Controller
    public interface IController : IBelongToArchitecture, ICanSendCommand, ICanGetSystem, ICanGetModel, ICanRegisterEvent, ICanSendQuery
    {

    }
    #endregion
    #region System
    public interface ISystem : IBelongToArchitecture, ICanSetArchitecture, ICanGetModel, ICanGetUtility, ICanRegisterEvent, ICanSendEvent
    {
        void Init();
    }
    public abstract class AbstractSystem : ISystem
    {
        private IArchitecture mArchitecture;
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return mArchitecture;
        }
        void ISystem.Init()
        {
            OnInit();
        }
        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }
        protected abstract void OnInit();
    }
    #endregion
    #region Model
    public interface IModel : IBelongToArchitecture, ICanSetArchitecture, ICanGetUtility, ICanSendEvent
    {
        void Init();
    }
    public abstract class AbstractModel : IModel
    {
        private IArchitecture mArchitecture;
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return mArchitecture;
        }

        void IModel.Init()
        {
            OnInit();
        }

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }
        protected abstract void OnInit();
    }
    #endregion
    #region Utility
    public interface IUtility
    {

    }
    #endregion
    #region Command
    public interface ICommand : IBelongToArchitecture, ICanSetArchitecture, ICanGetModel, ICanGetSystem, ICanGetUtility, ICanSendEvent, ICanSendCommand, ICanSendQuery
    {
        public void Execute();
    }
    public abstract class AbstractCommand : ICommand
    {
        private IArchitecture mArchitecture;
        void ICommand.Execute()
        {
            OnExecute();
        }
        protected virtual void OnExecute()
        {

        }
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return mArchitecture;
        }

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }
    }
    #endregion
    #region Query
    public interface IQuery<TResult> : IBelongToArchitecture, ICanSetArchitecture, ICanGetModel, ICanGetSystem, ICanSendQuery
    {
        TResult Do();
    }
    public abstract class AbstractQuery<T> : IQuery<T>
    {
        public T Do()
        {
            return OnDo();
        }
        protected abstract T OnDo();
        private IArchitecture mArchitecture;
        public IArchitecture GetArchitecture()
        {
            return mArchitecture;
        }

        public void SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }
    }
    #endregion
    #region Rule
    public interface IBelongToArchitecture
    {
        IArchitecture GetArchitecture();
    }
    public interface ICanSetArchitecture
    {
        void SetArchitecture(IArchitecture architecture);
    }
    public interface ICanGetModel : IBelongToArchitecture
    {

    }
    public static class CanGetModelExtension
    {
        public static T GetModel<T>(this ICanGetModel self) where T : class, IModel
        {
            return self.GetArchitecture().GetModel<T>();
        }
    }
    public interface ICanGetSystem : IBelongToArchitecture
    {

    }
    public static class CanGetSystemExtension
    {
        public static T GetSystem<T>(this ICanGetSystem self) where T : class, ISystem
        {
            return self.GetArchitecture().GetSystem<T>();
        }
    }
    public interface ICanGetUtility : IBelongToArchitecture
    {

    }
    public static class CanGetUtilityExtension
    {
        public static T GetUtility<T>(this ICanGetUtility self) where T : class, IUtility
        {
            return self.GetArchitecture().GetUtility<T>();
        }
    }
    public interface ICanRegisterEvent : IBelongToArchitecture
    {

    }
    public static class CanRegisterEventExtension
    {
        public static IUnRegister RegisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent)
        {
            return self.GetArchitecture().RegisterEvent<T>(onEvent);
        }
        public static void UnRegisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent)
        {
            self.GetArchitecture().UnRegisterEvent<T>(onEvent);
        }
    }
    public interface ICanSendCommand : IBelongToArchitecture, ICanSendQuery
    {

    }
    public static class CanSendCommandExtension
    {
        public static void SendCommand<T>(this ICanSendCommand self) where T : ICommand, new()
        {
            self.GetArchitecture().SendCommand<T>();
        }
        public static void SendCommand<T>(this ICanSendCommand self, T command) where T : ICommand
        {
            self.GetArchitecture().SendCommand<T>(command);
        }
    }
    public interface ICanSendEvent : IBelongToArchitecture
    {

    }
    public static class CanSendEventExtension
    {
        public static void SendEvent<T>(this ICanSendEvent self) where T : new()
        {
            self.GetArchitecture().SendEvent<T>();
        }
        public static void SendEvent<T>(this ICanSendEvent self, T e)
        {
            self.GetArchitecture().SendEvent<T>(e);
        }

    }
    public interface ICanSendQuery : IBelongToArchitecture
    {

    }
    public static class CanSendQueryExtension
    {
        public static TResult SendQuery<TResult>(this ICanSendQuery self, IQuery<TResult> query)
        {
            return self.GetArchitecture().SendQuery(query);
        }
    }
    #endregion
    #region TypeEventSystem
    public interface ITypeEventSystem
    {
        void Send<T>() where T : new();
        void Send<T>(T e);
        IUnRegister Register<T>(Action<T> onEvent);
        void UnRegister<T>(Action<T> onEvent);
    }
    public interface IUnRegister
    {
        void UnRegister();
    }
    public struct TypeEventSystemUnRegister<T> : IUnRegister
    {
        public ITypeEventSystem typeEventSystem;
        public Action<T> OnEvent;

        public void UnRegister()
        {
            typeEventSystem.UnRegister<T>(OnEvent);
            typeEventSystem = null;
            OnEvent = null;
        }

    }
    public class UnRegisterOnDestroyTrigger : MonoBehaviour
    {
        private HashSet<IUnRegister> mUnRegisters = new HashSet<IUnRegister>();
        public void AddUnRegister(IUnRegister unRegister)
        {
            mUnRegisters.Add(unRegister);
        }
        private void OnDestroy()
        {
            foreach (var unregister in mUnRegisters)
            {
                unregister.UnRegister();
            }
            mUnRegisters.Clear();
        }
    }
    public static class UnRegisterExtension
    {
        public static void UnRegisterWhenGameObjectDestroyed(this IUnRegister unRegister, GameObject gameObject)
        {
            var trigger = gameObject.GetComponent<UnRegisterOnDestroyTrigger>();
            if (!trigger)
            {
                trigger = gameObject.AddComponent<UnRegisterOnDestroyTrigger>();
            }
            trigger.AddUnRegister(unRegister);
        }
    }    
    public class TypeEventSystem : ITypeEventSystem
    {
        public interface IRegisterations
        {

        }
        public class Registerations<T> : IRegisterations
        {
            public Action<T> OnEvent = e => { };
        }
        public static readonly TypeEventSystem Global = new TypeEventSystem();
        public IUnRegister Register<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegisterations registerations;
            if (mEventRegisteration.TryGetValue(type, out registerations))
            {

            }
            else
            {
                registerations = new Registerations<T>();
                mEventRegisteration.Add(type, registerations);
            }
            (registerations as Registerations<T>).OnEvent += onEvent;
            return new TypeEventSystemUnRegister<T>()
            {
                OnEvent = onEvent,
                typeEventSystem = this
            };
        }
        private Dictionary<Type, IRegisterations> mEventRegisteration = new Dictionary<Type, IRegisterations>();        
        public void Send<T>() where T : new()
        {
            var e = new T();
            Send<T>(e);
        }        
        public void Send<T>(T e)
        {
            var type = typeof(T);
            IRegisterations registerations;
            if (mEventRegisteration.TryGetValue(type, out registerations))
            {
                (registerations as Registerations<T>).OnEvent(e);
            }
        }

        public void UnRegister<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegisterations registerations;
            if (mEventRegisteration.TryGetValue(type, out registerations))
            {
                (registerations as Registerations<T>).OnEvent -= onEvent;
            }
        }       
    }
    public interface IOnEvent<T>
    {
        void OnEvent(T e);
    }
    public static class OnGlobalEventEctension
    {
        public static IUnRegister RegisterEvent<T>(this IOnEvent<T> self) where T : struct
        {
            return TypeEventSystem.Global.Register<T>(self.OnEvent);
        }
        public static void UnRegisterEvent<T>(this IOnEvent<T> self) where T : struct
        {
            TypeEventSystem.Global.UnRegister<T>(self.OnEvent);
        }
    }    
    #endregion
    #region IOCCOntainer
    /// <summary>
    /// 创建IOC容器
    /// </summary>
    public class IOCContainer
    {
        public Dictionary<Type, object> mInstance = new Dictionary<Type, object>();
        public void register<T>(T instacne)
        {
            var key = typeof(T);
            if (mInstance.ContainsKey(key))
                mInstance[key] = instacne;
            else
                mInstance.Add(key, instacne);
        }

        public T Get<T>() where T : class
        {
            var key = typeof(T);
            if (mInstance.TryGetValue(key, out var retInstance))
                return retInstance as T;
            return null;
        }

    }
    #endregion
    #region BindableProperty
    /// <summary>
    /// 可绑定的属性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BindableProperty<T>
    {
        public BindableProperty(T defaultValue = default)
        {
            mValue = defaultValue;
        }
        /// <summary>
        /// 对于引用类型，默认值是 null，对于值类型，默认值是其零值（zero value），例如 0、false 或者是 struct 类型的实例的默认构造函数所生成的值。
        /// </summary>
        public T mValue = default(T);
        public T Value
        {
            get { return mValue; }
            set
            {
                if (value == null && mValue == null) return;
                if (value != null && value.Equals(mValue)) return;
                mValue = value;
                OnValueChanged?.Invoke(mValue);
                //?. 是 C# 中的空值条件运算符，用于在 OnValueChanged 不为 null 时调用 Invoke 方法。这样做可以避免空引用异常。
            }
        }
        /// <summary>
        /// 它实际上是一个空的方法体，用于初始化 OnValueChanged。也就是说，如果没有其他方法注册到 OnValueChanged 事件，它将不做任何事情。
        /// </summary>
        public Action<T> OnValueChanged = (v) => { };
        public IUnRegister Register(Action<T> onValueChanged)
        {
            OnValueChanged += onValueChanged;
            return new BindablePropertyUnRegister<T>()
            {
                BindableProperty = this,
                OnValueChanged = onValueChanged
            };
        }
        public IUnRegister RegisterWithInitValue(Action<T> onValueChanged)
        {
            onValueChanged(mValue);
            return Register(onValueChanged);
            OnValueChanged += onValueChanged;
            return new BindablePropertyUnRegister<T>()
            {
                BindableProperty = this,
                OnValueChanged = onValueChanged
            };
        }
        /// <summary>
        /// 改进值比较 a.value ==b.value  改进后为a==b
        /// </summary>
        /// <param name="property"></param>
        public static implicit operator T(BindableProperty<T> property)
        {
            return property.Value;
        }
        public override string ToString()
        {
            return Value.ToString();
        }
        public void UnRegister(Action<T> onValueChanged)
        {
            OnValueChanged -= onValueChanged;
        }
    }
    public class BindablePropertyUnRegister<T> : IUnRegister
    {
        public BindableProperty<T> BindableProperty { get; set; }
        public Action<T> OnValueChanged { get; set; }
        public void UnRegister()
        {
            BindableProperty.UnRegister(OnValueChanged);
            BindableProperty = null;
            OnValueChanged = null;
        }
    }
    #endregion

}
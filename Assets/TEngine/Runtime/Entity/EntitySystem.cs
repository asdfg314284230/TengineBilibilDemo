﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TEngine.EntityModule
{
    /// <summary>
    /// Entity系统
    /// </summary>
    [Serializable]
    public class EntitySystem : IDisposable
    {
        private static EntitySystem _instance;

        public static EntitySystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EntitySystem();
                }

                return _instance;
            }
        }
        private EntitySystem()
        {
            GameEventMgr.Instance.AddEventListener<int,Action<EcsObject>>(EntityEvent.AttachToEntity, AttachToEntity);
            //Update(true);
            MonoUtility.AddUpdateListener((() =>
            {
                Update(false);
            }));
            MonoUtility.AddFixedUpdateListener(FixedUpdate);
            MonoUtility.AddLateUpdateListener(LateUpdate);
        }
        /// <summary>
        /// Key -> HashSet(type) / Value -> Stack(EcsObject)
        /// </summary>
        internal readonly Dictionary<int, Stack<EcsObject>> ObjectPool = new Dictionary<int, Stack<EcsObject>>();
        internal readonly ArrayPool<Entity> Entities = new ArrayPool<Entity>();
        internal readonly Dictionary<int, EcsObject> EcsObjects = new Dictionary<int, EcsObject>();
        internal int CurInstanceId = 1000;

        #region internal
        internal void AddEntity(Entity entity)
        {
            entity.System = this;
            entity.Awake();
            Entities.Add(entity);
        }

        internal void RemoveEntity(Entity entity)
        {
            Entities.Buckets[entity.Index] = false;
        }

        private T Get<T>() where T : EcsObject, new()
        {
            int type = typeof(T).GetHashCode();
            if (ObjectPool.TryGetValue(type, out Stack<EcsObject> stack))
            {
                if (stack.Count > 0)
                {
                    var poolObj = (T)stack.Pop();
                    GenInstanceId(poolObj);
                    return poolObj;
                }
                goto Instantiate;
            }
            stack = new Stack<EcsObject>();
            ObjectPool.Add(type, stack);
            Instantiate: T ecsObject = new T();
            GenInstanceId(ecsObject);
            return ecsObject;
        }

        private void GenInstanceId(EcsObject ecsObject)
        {
            ecsObject.InstanceId = CurInstanceId;
            Instance.CurInstanceId++;
            EcsObjects.Add(ecsObject.InstanceId, ecsObject);
        }

        internal void Push(EcsObject ecsObject)
        {
            int type = ecsObject.HashCode;
            if (ObjectPool.TryGetValue(type, out Stack<EcsObject> stack))
            {
                stack.Push(ecsObject);
                return;
            }
            stack = new Stack<EcsObject>();
            ObjectPool.Add(type, stack);
            stack.Push(ecsObject);
        }
        internal T Create<T>() where T : Entity, new()
        {
            T entity = Get<T>();
            entity.IsDispose = false;
            AddEntity(entity);
            return entity;
        }
        internal T CreateComponent<T>() where T : EntityComponent, new()
        {
            T component = Get<T>();
            return component;
        }
        #endregion

        #region Update
        /// <summary>
        /// 更新Entity系统
        /// </summary>
        /// <param name="worker">线程池是否并行</param>
        public void Update(bool worker = false)
        {
            int count = Entities.Count;
            if (!worker)
            {
                for (int i = 0; i < count; i++)
                {
                    if (!Entities.Buckets[i])
                    {
                        continue;
                    }

                    if (!Entities[i].CanUpdate)
                    {
                        continue;
                    }
                    Entities[i].Update();
                }
            }
            else
            {
                Parallel.For(0, count, i =>
                {
                    if (!Entities.Buckets[i])
                    {
                        return;
                    }

                    if (!Entities[i].CanUpdate)
                    {
                        return;
                    }
                    Entities[i].Update();
                });
            }
        }

        public void FixedUpdate()
        {
            int count = Entities.Count;
            for (int i = 0; i < count; i++)
            {
                if (!Entities.Buckets[i])
                {
                    continue;
                }

                if (!Entities[i].CanFixedUpdate)
                {
                    continue;
                }
                Entities[i].FixedUpdate();
            }
        }

        public void LateUpdate()
        {
            int count = Entities.Count;
            for (int i = 0; i < count; i++)
            {
                if (!Entities.Buckets[i])
                {
                    continue;
                }

                if (!Entities[i].CanLateUpdates)
                {
                    continue;
                }
                Entities[i].LateUpdate();
            }
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            _instance = null;
        }
        #endregion

        #region FindObjectOfType
        public T FindObjectOfType<T>() where T : EcsObject
        {
            Type type = typeof(T);
            var elements = Entities.ToArray();
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i].GetType() == type)
                {
                    return elements[i] as T;
                }

                for (int j = 0; j < elements[i].Components.Count; j++)
                {
                    if (elements[i].Components[j].GetType() == type)
                    {
                        return elements[i].Components[j] as T;
                    }
                }
            }
            return null;
        }

        public T[] FindObjectsOfType<T>() where T : EcsObject
        {
            Type type = typeof(T);
            var entities = Entities.ToArray();
            List<T> elements = new List<T>();
            for (int i = 0; i < entities.Length; i++)
            {
                if (entities[i].GetType() == type)
                {
                    elements.Add(entities[i] as T);
                }
                for (int n = 0; n < entities[i].Components.Count; n++)
                {
                    if (entities[i].Components[n].GetType() == type)
                    {
                        elements.Add(entities[i].Components[n] as T);
                    }
                }
            }
            return elements.ToArray();
        }
        #endregion

        #region AttachEventToEntity

        public void AttachToEntity(int instanceId,Action<EcsObject> callback)
        {
            if (EcsObjects.TryGetValue(instanceId,out var entity))
            {
                if (entity.IsDisposed)
                {
                    return;
                }
                callback?.Invoke(entity);
            }
        }
        

        #endregion
    }

    public static class EntityEvent
    {
        public static int AttachToEntity = StringId.StringToHash("EntityEvent.AttachToEntity");
    }
}
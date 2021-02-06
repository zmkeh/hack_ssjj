using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ssjj_hack
{
    public abstract class ModuleBase
    {
        public virtual void Awake()
        {

        }

        public virtual void Start()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void FixedUpdate()
        {

        }

        public virtual void LateUpdate()
        {

        }

        public virtual void OnGUI()
        {

        }

        public virtual void OnDestroy()
        {

        }

        protected static Type GetTypeMain(string fullname)
        {
            return GetAssembly("Assembly-CSharp").GetType(fullname);
        }

        protected static Assembly GetAssembly(string name)
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (name == asm.FullName.Substring(0, asm.FullName.IndexOf(",")))
                    return asm;
            }
            return null;
        }
    }
}

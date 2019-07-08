using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//简单工厂  
namespace TDE
{
    //创建敌人选择器
    public class SelectorFactory
    {
        //攻击目标选择器缓存
        private static Dictionary<string, IUIScaleDrag> cache = new Dictionary<string, IUIScaleDrag>();

        public static IUIScaleDrag CreateUIScaleSelector(ScaleCenter mode)
        {
            //没有缓存则创建
            if (!cache.ContainsKey(mode.ToString()))
            {
                var nameSpace = typeof(SelectorFactory).Namespace;
                string classFullName = string.Format("UI{0}Drag", mode.ToString());

                if (!String.IsNullOrEmpty(nameSpace))
                    classFullName = nameSpace + "." + classFullName;

                Type type = Type.GetType(classFullName);
                cache.Add(mode.ToString(), Activator.CreateInstance(type) as IUIScaleDrag);
            }
            //从缓存中取得创建好的选择器对象
            return cache[mode.ToString()];
        }
    }
}

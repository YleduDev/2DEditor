using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//�򵥹���  
namespace TDE
{
    //��������ѡ����
    public class SelectorFactory
    {
        //����Ŀ��ѡ��������
        private static Dictionary<string, IUIScaleDrag> cache = new Dictionary<string, IUIScaleDrag>();

        public static IUIScaleDrag CreateUIScaleSelector(ScaleCenter mode)
        {
            //û�л����򴴽�
            if (!cache.ContainsKey(mode.ToString()))
            {
                var nameSpace = typeof(SelectorFactory).Namespace;
                string classFullName = string.Format("UI{0}Drag", mode.ToString());

                if (!String.IsNullOrEmpty(nameSpace))
                    classFullName = nameSpace + "." + classFullName;

                Type type = Type.GetType(classFullName);
                cache.Add(mode.ToString(), Activator.CreateInstance(type) as IUIScaleDrag);
            }
            //�ӻ�����ȡ�ô����õ�ѡ��������
            return cache[mode.ToString()];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace CommonLibrary
{
    public class WPFAutomationUtil
    {
        public static AutomationElement GetElement(AutomationElement parentElement, string value)
        {
            Condition condition = new PropertyCondition(AutomationElement.AutomationIdProperty, value);
            AutomationElement Element = parentElement.FindFirst(TreeScope.Descendants, condition);
            return Element;
        }

        public static AutomationElement GetElementByName(AutomationElement parentElement, string value)
        {
            Condition condition = new PropertyCondition(AutomationElement.NameProperty, value);
            AutomationElement Element = parentElement.FindFirst(TreeScope.Descendants, condition);
            return Element;
        }

        public static GridPattern GetGridPattern(AutomationElement element)
        {
            object currentPattern;
            if (!element.TryGetCurrentPattern(GridPattern.Pattern, out currentPattern))
            {
                throw new Exception(string.Format("Element with AutomationId '{0}' and Name '{1}' does not support the GridPattern.",
                    element.Current.AutomationId, element.Current.Name));
            }
            return currentPattern as GridPattern;
        }

        public static ValuePattern GetValuePattern(AutomationElement element)
        {
            object currentPattern;
            if (!element.TryGetCurrentPattern(ValuePattern.Pattern, out currentPattern))
            {
                throw new Exception(string.Format("Element with AutomationId '{0}' and Name '{1}' does not support the ValuePattern.",
                    element.Current.AutomationId, element.Current.Name));
            }
            return currentPattern as ValuePattern;
        }

        public static InvokePattern GetAutomationPattern(AutomationElement element)
        {
            object currentPattern;
            if (!element.TryGetCurrentPattern(InvokePattern.Pattern, out currentPattern))
            {
                throw new Exception(string.Format("Element with AutomationId '{0}' and Name '{1}' does not support the ValuePattern.",
                    element.Current.AutomationId, element.Current.Name));
            }
            return currentPattern as InvokePattern;
        }
    }
}

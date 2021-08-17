using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vwm.RTree.Api.Attributes
{
  [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
  public class MaxValueAttribute: ValidationAttribute
  {
    private readonly double fMaxValue;

    public MaxValueAttribute(double maxValue)
    {
      fMaxValue = maxValue;
    }

    public override bool IsValid(object value)
    {
      if (value == null)
        return true;

      if (value is IConvertible)
      {
        try
        {
          return Convert.ToDouble(value) <= fMaxValue;
        }
        catch (OverflowException)
        {
          return false;
        }
      }

      throw new InvalidOperationException("Wrong usage of attribute.");
    }

    public override string FormatErrorMessage(string name)
    {
      return $"The value of {name} is greater than {fMaxValue}.";
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Projector
{
    public class RefScrMath
    {
       
        private RefScrMathObj mathBase;

        private ReflectionScript parentScript;

        private Boolean castError = false;


        

        public RefScrMath(ReflectionScript parent,string Name,string code)
        {
            this.parentScript = parent;
            this.mathBase = new RefScrMathObj();
            this.mathBase.name = Name;
            this.mathBase.code = code;
            this.mathBase.Value = 0;
            
        }

        public String getResult()
        {
            return this.mathBase.Value.ToString();
        }


        public void calc()
        {
            this.addition();            
        }

        private double tryCast(string str)
        {
            double result = 0;
            this.castError = true;
            try
            {
                result = Double.Parse(str);
            }
            catch (Exception e)
            {
                return 0;
            }
            this.castError = false;
            return result;
        }


        // the last caclutaion will be the entrypoint
        private void  addition()
        {
            string[] additionParts = this.mathBase.code.Split(new char[] {'+'}, StringSplitOptions.RemoveEmptyEntries);
            Boolean calc = false;
            foreach (string addPart in additionParts)
            {
                string parsingStr = this.parentScript.fillUpAll(addPart);
                double getVal = this.substract( parsingStr );
                //double getVal = this.tryCast( parsingStr );
                if (!this.castError)
                {
                    if (!calc)
                    {
                        this.mathBase.Value = getVal;
                    }
                    else
                    {
                        this.mathBase.Value += getVal;
                    }


                    calc = true;
                }
            }
        }




        private double substract(string part)
        {
            double firstCheck = this.tryCast(part);
            if (!this.castError)
            {
                return firstCheck;
            }
            string[] subPartsParts = part.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            
            double val = 0;
            Boolean calc = false;
            foreach (string subPart in subPartsParts)
            {
                double getVal = this.mutliplication(subPart);
                if (!this.castError)
                {
                    if (!calc)
                    {
                        val = getVal;
                    }
                    else
                    {
                        val -= getVal;
                    }
                    calc = true;
                }
               
            }
            return val;
        }

        private double mutliplication(string part)
        {
            double firstCheck = this.tryCast(part);
            if (!this.castError)
            {
                return firstCheck;
            }
            double val = 0;
            Boolean calc = false;
            string[] subPartsParts = part.Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string subPart in subPartsParts)
            {
                double getVal = this.division(subPart);
                if (!this.castError)
                {
                    if (!calc)
                    {
                        val = getVal;
                    }
                    else
                    {
                        val *= getVal;
                    }
                    calc = true;
                }
                
            }
            return val;
        }

        private double division(string part)
        {
            double firstCheck = this.tryCast(part);
            if (!this.castError)
            {
                return firstCheck;
            }
            double val = 0;
            Boolean calcualtion = false;
            string[] subParts = part.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string subPart in subParts)
            {
                double getVal = this.tryCast(subPart);
                if (!this.castError)
                {
                    if (!calcualtion)
                    {
                        val = getVal;
                    }
                    else
                    {
                        val /= getVal;
                    }
                    calcualtion = true;
                }
            }

            return val;
        }
    }
}

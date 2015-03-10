using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Projector.Script
{
    public class RefScrMath
    {
        /// <summary>
        /// base prop object
        /// </summary>
        private RefScrMathObj mathBase;

        /// <summary>
        /// parent script that uses "me"
        /// </summary>
        private ReflectionScript parentScript;

        /// <summary>
        /// some casting error
        /// </summary>
        private Boolean castError = false;


        
        /// <summary>
        /// Constructs the Math object.
        /// 
        /// </summary>
        /// <param name="parent">The script that uses the the math</param>
        /// <param name="Name">Name of the Calculation</param>
        /// <param name="code">the code that contains the calculation</param>
        public RefScrMath(ReflectionScript parent,string Name,string code)
        {
            this.parentScript = parent;
            this.mathBase = new RefScrMathObj();
            this.mathBase.name = Name;
            this.mathBase.code = code;
            this.mathBase.Value = 0;
            
        }

        /// <summary>
        /// returns the result of the calculation
        /// </summary>
        /// <returns></returns>
        public double getResult()
        {
            return this.mathBase.Value;
        }

        /// <summary>
        /// start the caclutaion by addition. this will
        /// trigger the net calculation
        /// </summary>
        public void calc()
        {
            this.addition();            
        }

        /// <summary>
        /// try to cast an string into an number
        /// </summary>
        /// <param name="str">the param as string</param>
        /// <returns>the casted result or 0 if casting fails</returns>
        private double tryCast(string str)
        {
            double result = 0;
            this.castError = true;
            try
            {
                result = Double.Parse(str);
            }
            catch (Exception)
            {
                return 0;
            }
            this.castError = false;
            return result;
        }


        /// <summary>
        /// handle all additions and triggers the subtraction
        /// </summary>
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



        /// <summary>
        /// Handle all sunstractions and triggers the 
        /// multiplication
        /// </summary>
        /// <param name="part">the code that is not parsed already</param>
        /// <returns></returns>
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

        /// <summary>
        /// handle all mutliplications and triggers the 
        /// substractions
        /// </summary>
        /// <param name="part">the code that is not parsed already</param>
        /// <returns></returns>
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

        /// <summary>
        /// handles als division and finishs the regular calculation
        /// </summary>
        /// <param name="part">the code that is not parsed already</param>
        /// <returns></returns>
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

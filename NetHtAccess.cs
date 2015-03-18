using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Projector.Net.Secured
{
    /// <summary>
    /// Class to connect to a with HTACCESS secured website 
    /// 
    /// </summary>
    public class NetHtAccess
    {


        private string UserName = "";
        private string PassWord = "";
        private string BaseDomain = "";
        private string callUri = "";

        private string content = "";

        private string loadErrorMessage = "";

        private string sendMethod = "POST";
        private string contentType = "application/x-www-form-urlencoded";

        /// <summary>
        /// contains all parameters that should be send
        /// </summary>
        private Hashtable parameters = new Hashtable();

        /// <summary>
        /// sets the user by Username and Password
        /// </summary>
        /// <param name="username">the Username tologin into HTACCESS</param>
        /// <param name="password">the password for the user</param>
        public void setUser(string username, string password)
        {
            this.UserName = username;
            this.PassWord = password;
        }

        /// <summary>
        /// returns the last loaded content
        /// </summary>
        /// <returns>the content from website</returns>
        public string getContent()
        {
            return this.content;
        }

        /// <summary>
        /// sets the basedomain like http://www.google.com
        /// </summary>
        /// <param name="domain">the base domain</param>
        public void setBaseDomain(string domain)
        {
            this.BaseDomain = domain;
        }

        /// <summary>
        /// sets the full url
        /// that would be requested
        /// </summary>
        /// <param name="url"></param>
        public void setUri(string url)
        {
            this.callUri = url;
        }

        /// <summary>
        /// resets all Parameters
        /// </summary>
        public void resetParameters()
        {
            this.parameters.Clear();
        }

        /// <summary>
        /// returns the last error message
        /// </summary>
        /// <returns></returns>
        public string getLastError()
        {
            return this.loadErrorMessage;
        }

        /// <summary>
        /// adds or replace existing string paramaters
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void addOrReplaceParam(string name, string value)
        {
            if (this.parameters.ContainsKey(name))
            {
                this.parameters[name] = value;
            }
            else
            {
                this.parameters.Add(name, value);
            }
        }

        /// <summary>
        /// builds webRequest parameters
        /// </summary>
        /// <param name="wr"></param>
        private void setPostData(WebRequest wr)
        {
            
            NameValueCollection outgoingQueryString = HttpUtility.ParseQueryString(String.Empty);
            string add = "";
            foreach (DictionaryEntry req in this.parameters)
            {
                //postData += add + req.Key.ToString() + "=" + req.Value.ToString();
                //add = "&";

                if (req.Value.ToString() != "")
                    outgoingQueryString.Add(req.Key.ToString(), req.Value.ToString());
                
            }
            string postData = outgoingQueryString.ToString();
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            wr.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = wr.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
        }

        /// <summary>
        /// Load the content
        /// </summary>
        public void load()
        {
            NetworkCredential myCred = new NetworkCredential(this.UserName, this.PassWord);
            CredentialCache myCache = new CredentialCache();

            myCache.Add(new Uri(this.BaseDomain), "Basic", myCred);

            WebRequest request = (HttpWebRequest) WebRequest.Create(this.callUri);
            ((HttpWebRequest)request).UserAgent = "ProjectorRequest";
            request.PreAuthenticate = true;

            request.Method = WebRequestMethods.Http.Post;
            request.Credentials = myCache;
            request.ContentType = this.contentType;
            
            if (this.parameters.Count > 0)
            {
                //this.setPostData(request);
                NameValueCollection outgoingQueryString = HttpUtility.ParseQueryString(String.Empty);

                foreach (DictionaryEntry req in this.parameters)
                {
                    if (req.Value.ToString() != "")
                        outgoingQueryString.Add(req.Key.ToString(), req.Value.ToString());

                }
                string postData = outgoingQueryString.ToString();
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = byteArray.Length;                
                Stream dataStream = request.GetRequestStream();                
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Flush();    
                dataStream.Close();
            }


            try
            {
                WebResponse webResponse = request.GetResponse();

                Stream rStream = webResponse.GetResponseStream();
                StreamReader str = new StreamReader(rStream);
                if (str.EndOfStream != true)
                {
                    this.content = str.ReadToEnd();
                }
                webResponse.Close();
                rStream.Close();
                str.Close();


                webResponse.Close();
            }
            catch (Exception ex)
            {
                this.loadErrorMessage = ex.Message;
            }

        }
    }
}

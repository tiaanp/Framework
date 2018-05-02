using System;
using Epine.Infrastructure.Resolvers;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Epine.Infrastructure.Utilities {

	/// <summary>
	/// 
	/// </summary>
	public sealed class Http {

        #region Constant

        /// <summary>
        ///     text/plain
        /// </summary>
        private const string DEFAULT_CONTENT_TYPE = "application/json";

		#endregion


		#region Constructor

		public Http() {
		}

		#endregion


		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="uri"></param>
		/// <param name="postData"></param>
		/// <returns></returns>
		public TResult Post<TResult>(string uri, byte[] postData) {

            return this.Post<TResult>(uri, postData, DEFAULT_CONTENT_TYPE);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="uri"></param>
        /// <param name="postData"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public TResult Post<TResult>(string uri, byte[] postData, string contentType) {
                        
            var response = default(TResult);

            System.Net.ServicePointManager.Expect100Continue = false;

            // Create a request using a URL that can receive a post. 
            var request = (HttpWebRequest)WebRequest.Create(uri);
            HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            request.CachePolicy = noCachePolicy;
            // Set the Method property of the request to POST.
            request.Method = "POST";

            // Set the ContentType property of the WebRequest.
            request.ContentType = contentType;
            request.Timeout = 240000;
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = postData.Length;

            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(postData, 0, postData.Length);

            // Get the response.
            //   WebResponse webResponse = request.GetResponse();



            using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
            {
                if (webResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Thread Not Complete");
                }
                dataStream = webResponse.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);

                // Read the content.			
                response =
                    StreamReaderResolverFactory
                        .GetResolver<TResult>()
                        .GetResult(reader);

                reader.Close();
                dataStream.Close();
                webResponse.Close();

            }
            // Get the stream containing content returned by the server.
            //   dataStream = webResponse.GetResponseStream();

            // Open the stream using a StreamReader for easy access.
            //   StreamReader reader = new StreamReader(dataStream);

            // Read the content.			
            //response =
            //    StreamReaderResolverFactory
            //        .GetResolver<TResult>()
            //        .GetResult(reader);

            // Clean up the streams.			


            return response;
        }

        public TResult Post<TResult>(string uri, byte[] postData, string contentType,string method)
        {

            var response = default(TResult);

            System.Net.ServicePointManager.Expect100Continue = false;

            // Create a request using a URL that can receive a post. 
            var request = (HttpWebRequest)WebRequest.Create(uri);
            HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            request.CachePolicy = noCachePolicy;
            // Set the Method property of the request to POST.
            request.Method = "POST";

            // Set the ContentType property of the WebRequest.
            request.Method = method;
            request.ContentType = contentType;
            request.Timeout = 240000;
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = postData.Length;

            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(postData, 0, postData.Length);

            // Get the response.
            //   WebResponse webResponse = request.GetResponse();



            using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
            {
                if (webResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Thread Not Complete");
                }
                dataStream = webResponse.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);

                // Read the content.			
                response =
                    StreamReaderResolverFactory
                        .GetResolver<TResult>()
                        .GetResult(reader);

                reader.Close();
                dataStream.Close();
                webResponse.Close();

            }
            // Get the stream containing content returned by the server.
            //   dataStream = webResponse.GetResponseStream();

            // Open the stream using a StreamReader for easy access.
            //   StreamReader reader = new StreamReader(dataStream);

            // Read the content.			
            //response =
            //    StreamReaderResolverFactory
            //        .GetResolver<TResult>()
            //        .GetResult(reader);

            // Clean up the streams.			


            return response;
        }


        public TResult Post<TResult>(string uri, byte[] postData, string contentType,  WebHeaderCollection Headers)
        {

            var response = default(TResult);

            System.Net.ServicePointManager.Expect100Continue = false;

            // Create a request using a URL that can receive a post. 
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Accept = contentType;
            request.Headers = Headers;
            HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            request.CachePolicy = noCachePolicy;
            // Set the Method property of the request to POST.
            request.Method = "POST";

            // Set the ContentType property of the WebRequest.
           // request.Method = method;
            request.ContentType = contentType;
            request.Timeout = 240000;
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = postData.Length;

            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(postData, 0, postData.Length);

            // Get the response.
            //   WebResponse webResponse = request.GetResponse();



            using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
            {
                if (webResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Thread Not Complete");
                }
                dataStream = webResponse.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);

                // Read the content.			
                response =
                    StreamReaderResolverFactory
                        .GetResolver<TResult>()
                        .GetResult(reader);

                reader.Close();
                dataStream.Close();
                webResponse.Close();

            }
            // Get the stream containing content returned by the server.
            //   dataStream = webResponse.GetResponseStream();

            // Open the stream using a StreamReader for easy access.
            //   StreamReader reader = new StreamReader(dataStream);

            // Read the content.			
            //response =
            //    StreamReaderResolverFactory
            //        .GetResolver<TResult>()
            //        .GetResult(reader);

            // Clean up the streams.			


            return response;
        }

        public TResult Get<TResult>(string uri, string contentType, WebHeaderCollection Headers)
        {

            var response = default(TResult);

            System.Net.ServicePointManager.Expect100Continue = false;

            // Create a request using a URL that can receive a post. 
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Accept = contentType;
            request.Headers = Headers;
            HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            request.CachePolicy = noCachePolicy;
            // Set the Method property of the request to POST.
            request.Method = "GET";

            // Set the ContentType property of the WebRequest.
            // request.Method = method;
            request.ContentType = contentType;
            request.Timeout = 240000;
            // Set the ContentLength property of the WebRequest.
          //  request.ContentLength = postData.Length;

            // Get the request stream.
            Stream dataStream = new MemoryStream();
            //dataStream.Write(postData, 0, postData.Length);

            // Get the response.
            //   WebResponse webResponse = request.GetResponse();



            using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
            {
                if (webResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Thread Not Complete");
                }
                dataStream = webResponse.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);

                // Read the content.			
                response =
                    StreamReaderResolverFactory
                        .GetResolver<TResult>()
                        .GetResult(reader);

                reader.Close();
                dataStream.Close();
                webResponse.Close();

            }
            // Get the stream containing content returned by the server.
            //   dataStream = webResponse.GetResponseStream();

            // Open the stream using a StreamReader for easy access.
            //   StreamReader reader = new StreamReader(dataStream);

            // Read the content.			
            //response =
            //    StreamReaderResolverFactory
            //        .GetResolver<TResult>()
            //        .GetResult(reader);

            // Clean up the streams.			


            return response;
        }

	    public TResult Get<TResult>(string uri)
	    {

	        var response = default(TResult);

	        System.Net.ServicePointManager.Expect100Continue = false;

	        // Create a request using a URL that can receive a post. 
	        var request = (HttpWebRequest)WebRequest.Create(uri);
	        //request.Accept = contentType;
	        //request.Headers = Headers;
	        HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
	        request.CachePolicy = noCachePolicy;
	        // Set the Method property of the request to POST.
	        request.Method = "GET";

	        // Set the ContentType property of the WebRequest.
	        // request.Method = method;
	        //request.ContentType = contentType;
	        request.Timeout = 240000;
	        // Set the ContentLength property of the WebRequest.
	        //  request.ContentLength = postData.Length;

	        // Get the request stream.
	        Stream dataStream = new MemoryStream();
	        //dataStream.Write(postData, 0, postData.Length);

	        // Get the response.
	        //   WebResponse webResponse = request.GetResponse();



	        using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
	        {
	            if (webResponse.StatusCode != HttpStatusCode.OK)
	            {
	                throw new Exception("Thread Not Complete");
	            }
	            dataStream = webResponse.GetResponseStream();
	            StreamReader reader = new StreamReader(dataStream);

	            // Read the content.			
	            response =
	                StreamReaderResolverFactory
	                    .GetResolver<TResult>()
	                    .GetResult(reader);

	            reader.Close();
	            dataStream.Close();
	            webResponse.Close();

	        }
	        // Get the stream containing content returned by the server.
	        //   dataStream = webResponse.GetResponseStream();

	        // Open the stream using a StreamReader for easy access.
	        //   StreamReader reader = new StreamReader(dataStream);

	        // Read the content.			
	        //response =
	        //    StreamReaderResolverFactory
	        //        .GetResolver<TResult>()
	        //        .GetResult(reader);

	        // Clean up the streams.			


	        return response;
	    }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="uri"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public async Task<TResult> PostAsync<TResult>(string uri, byte[] postData) {

            return await this.PostAsync<TResult>(uri, postData, DEFAULT_CONTENT_TYPE);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="uri"></param>
        /// <param name="postData"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public async Task<TResult> PostAsync<TResult>(string uri, byte[] postData, string contentType) {

			var response = default(TResult);

			System.Net.ServicePointManager.Expect100Continue = false;

			// Create a request using a URL that can receive a post. 
			var request = (HttpWebRequest)WebRequest.Create(uri);

			// Set the Method property of the request to POST.
			request.Method = "POST";

			// Set the ContentType property of the WebRequest.
			request.ContentType = contentType;
			request.Timeout = 240000;
			// Set the ContentLength property of the WebRequest.
			request.ContentLength = postData.Length;

			// Get the request stream.
			Stream dataStream = await request.GetRequestStreamAsync();
			await dataStream.WriteAsync(postData, 0, postData.Length);

			// Get the response.
			;
            using (WebResponse webResponse = await request.GetResponseAsync())
            {
                dataStream = webResponse.GetResponseStream();

                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);

                // Read the content.			
                response =
                    StreamReaderResolverFactory
                        .GetResolver<TResult>()
                        .GetResult(reader);
                reader.Close();
                dataStream.Close();
                webResponse.Close();
            }
			// Get the stream containing content returned by the server.
			

			// Clean up the streams.			
			

			return response;
		}




        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="uri"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public TResult Delete<TResult>(string uri, byte[] postData) {

            return this.Delete<TResult>(uri, postData, DEFAULT_CONTENT_TYPE);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="uri"></param>
        /// <param name="postData"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public TResult Delete<TResult>(string uri, byte[] postData, string contentType) {

            var response = default(TResult);

            System.Net.ServicePointManager.Expect100Continue = false;

            // Create a request using a URL that can receive a post. 
            var request = (HttpWebRequest)WebRequest.Create(uri);

            // Set the Method property of the request to POST.
            request.Method = "DELETE";

            // Set the ContentType property of the WebRequest.
            request.ContentType = contentType;
            request.Timeout = 240000;
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = postData.Length;

            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(postData, 0, postData.Length);

            // Get the response.
            WebResponse webResponse = request.GetResponse();

            // Get the stream containing content returned by the server.
            dataStream = webResponse.GetResponseStream();

            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);

            // Read the content.			
            response =
                StreamReaderResolverFactory
                    .GetResolver<TResult>()
                    .GetResult(reader);

            // Clean up the streams.			
            reader.Close();
            dataStream.Close();
            webResponse.Close();

            return response;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        public void Call(string uri)
	    {
            System.Net.ServicePointManager.Expect100Continue = false;

            // Create a request using a URL that can receive a post. 
            var request = (HttpWebRequest)WebRequest.Create(uri);
	        request.GetResponse();
	    }

        public TResult PostClient<TResult>(string uri, byte[] postData)
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            var response = default(TResult);

            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage resp =
                               client.PostAsync(uri, new ByteArrayContent(postData))
                                   .Result;
           
            using (Stream dataStream = resp.Content.ReadAsStreamAsync().Result)
            {
                

                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);

                // Read the content.			
                response =
                    StreamReaderResolverFactory
                        .GetResolver<TResult>()
                        .GetResult(reader);
                reader.Close();
                dataStream.Close();
               
            }
          	


            return response;
        }
    }
}

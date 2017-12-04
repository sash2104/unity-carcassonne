using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Object = System.Object;

/// <summary>
/// Class D3VRest. A Wrapper class for simple and efficient REST API calls
/// http://www.debabhishek.com/simple-rest-utility-for-unity/
/// </summary>

namespace D3VRestWrapper
{
	class D3VRest : MonoBehaviour
	{
		void Start() { }

		/// <summary>
		/// Gets JSON data from the specified URL.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <param name="Callback">The callback.</param>
		public void GET(string url, System.Action<Object> Callback = null)
		{
			WWW www = new WWW(url);
			StartCoroutine(WaitForRequest(www, Callback));
		}

		/// <summary>
		/// Gets a texture form the specified URL.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <param name="Callback">The callback.</param>
		public void GETTexture(string url, System.Action<Object> Callback = null)
		{
			WWW www = new WWW(url);
			StartCoroutine(WaitForRequest(www, Callback, true));
		}

		/// <summary>
		/// GETs an asset from Asset Bundle
		/// </summary>
		/// <param name="url">The Url to fetch the Asset from</param>
		/// <param name="AssetName">Asset Name</param>
		/// <param name="Callback">The Callback that contains the GameObject as its parameter</param>
		public void GETAsset(string url, string AssetName, System.Action<Object> Callback = null)
		{
			StartCoroutine(WaitForAsset(url, AssetName, Callback));
		}


		/// <summary>
		/// Posts a Form data to the specified URL.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <param name="post">The post.</param>
		/// <param name="Callback">The callback.</param>
		/// <returns>UnityEngine.WWW.</returns>
		public WWW POST(string url, Dictionary<string, string> post, System.Action<Object> Callback = null)
		{
			WWWForm form = new WWWForm();
			foreach (KeyValuePair<String, String> post_arg in post)
			{
				form.AddField(post_arg.Key, post_arg.Value);
			}
			WWW www = new WWW(url, form);

			StartCoroutine(WaitForRequest(www, Callback));
			return www;
		}



		/// <summary>
		/// Waits for request.
		/// </summary>
		/// <param name="www">The WWW.</param>
		/// <param name="Callback">The callback.</param>
		/// <param name="isTexture">This is texture or not.</param>
		/// <returns>System.Collections.IEnumerator.</returns>
		private IEnumerator WaitForRequest(WWW www, System.Action<System.Object> Callback = null, bool isTexture = false)
		{
			yield return www;
			Object r;
			if (www.error == null)
			{
				//Ok
				if (!isTexture)
					r = "{err:false,data:" + www.text + "}";
				else
					r = www.texture;

				if (Callback != null)
					Callback(r);
			}
			else
			{
				//send error
				r = "{err:\"" + www.error + "\",data:false}";
				if (Callback != null)
					Callback(r);
			}
		}

		/// <summary>
		/// Generic Asset Downloader ( Non Cached Version)
		/// </summary>
		/// <returns>System.Object as Parameter to callback</returns>
		private IEnumerator WaitForAsset(string BundleURL, string AssetName, System.Action<System.Object> Callback = null, bool isTexture = false)
		{
			//  string BundleURL = "localhost:3000/prefabs/t1.unity3d";
			// string AssetName = "t1";


			using (WWW www = new WWW(BundleURL))
			{
				yield return www;
				AssetBundle bundle;


				if (www.error == null)
				{
					//Ok
					bundle = www.assetBundle;

					if (Callback != null)
					{
						if (AssetName == "")
							Callback(bundle.mainAsset);
						else
							Callback(bundle.LoadAsset(AssetName));
					}
					bundle.Unload(false);
				}
				else
				{
					//send error
					Object r = "{err:\"" + www.error + "\",data:false}";
					if (Callback != null)
						Callback(r);
				}


			} // memory is freed from the web stream (www.Dispose() gets called implicitly)
		}

	}

}

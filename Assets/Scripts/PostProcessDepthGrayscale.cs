/** Shamlessly stolen from http://willychyr.com/2013/11/unity-shaders-depth-and-normal-textures/ **/

using UnityEngine;
using System.Collections;

//so that we can see changes we make without having to run the game

[ExecuteInEditMode]
public class PostProcessDepthGrayscale : MonoBehaviour
{

	public Material mat;

	void Start()
	{
		GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, mat);
		//mat is the material which contains the shader
		//we are passing the destination RenderTexture to
	}
}
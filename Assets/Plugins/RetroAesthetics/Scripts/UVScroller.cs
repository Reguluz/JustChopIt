using UnityEngine;

namespace RetroAesthetics {
	public class UVScroller : MonoBehaviour {
		public Vector2 scrollSpeed = new Vector2(-1f, 0f);
		public string textureName = "_GridTex";
		private Material target;
		private Vector2 offset = Vector2.zero;

		void Start () {
			var renderer = GetComponent<Renderer>();
			if (renderer == null || renderer.material == null) {
				this.enabled = false;
				return;
			}

			target = renderer.material;
			if (!target.HasProperty(textureName)) {
				Debug.LogWarning("Texture name '" + textureName + "' not found in material.");
				this.enabled = false;
				return;
			}
		}
		
		void Update () {
			offset += scrollSpeed * Time.deltaTime * Application.targetFrameRate;
			target.SetTextureOffset(textureName, offset);
		}
	}
}
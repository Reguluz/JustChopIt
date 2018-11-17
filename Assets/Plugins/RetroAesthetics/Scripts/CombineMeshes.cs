using UnityEngine;

namespace RetroAesthetics {
	[ExecuteInEditMode]
	public class CombineMeshes : MonoBehaviour {

		[InspectorButton("CombineButtonHandler")]
		public bool combineChildMeshes;

		private void CombineButtonHandler() {
			CombineChildMeshes();
		}

		public virtual void CombineChildMeshes() {
			// Zero transformation is needed because of localToWorldMatrix transform
			Vector3 position = transform.position;
			transform.position = Vector3.zero;

			MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
			CombineInstance[] combine = new CombineInstance[meshFilters.Length];
			Material materialInChild = null;
			for (int i = 0; i < meshFilters.Length; ++i) {
				combine[i].mesh = meshFilters[i].sharedMesh;
				combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
				var renderer = meshFilters[i].gameObject.GetComponent<Renderer>();
				if (renderer != null) {
					renderer.enabled = false;
				}
				if (materialInChild == null && renderer.sharedMaterial != null) {
					materialInChild = renderer.sharedMaterial;
				}
			}

			// Add mesh filter.
			var meshFilter = GetComponent<MeshFilter>();
			if (meshFilter == null) {
				meshFilter = gameObject.AddComponent<MeshFilter>();
			}
			meshFilter.mesh = new Mesh();
			meshFilter.sharedMesh.CombineMeshes(combine, true, true);

			// Add renderer and set material.
			var meshRenderer = GetComponent<MeshRenderer>();
			if (meshRenderer == null) {
				meshRenderer = gameObject.AddComponent<MeshRenderer>();
				meshRenderer.sharedMaterial = materialInChild;
			}

			// Reset position
			transform.position = position;
		}
		
	}
}
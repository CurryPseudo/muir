using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerMaskUtil{
	public static bool IsIncluded(LayerMask mask, int layer) {
		layer = 1 << layer;
		int result = mask & layer;
		return result != 0;
	}
}

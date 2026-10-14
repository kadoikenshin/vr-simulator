using System.Numerics;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
  // 移動速度
  public float moveSpeed = 5.0f;

  void Update()
  {
    // Horizontal: 左右キー
    float moveX = 0f;

    // Vertical:  前後キー
    float moveZ = 0f;

    if (Keyboard.current != null)
    {
      if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveX = -1f;
      if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveX = 1f;
      if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveZ = -1f;
      if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveZ = 1f;
    }

    //移動する方向と距離の計算
    UnityEngine.Vector3 moveDir = new UnityEngine.Vector3(moveX, 0f, moveZ);

    transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);
  }

}

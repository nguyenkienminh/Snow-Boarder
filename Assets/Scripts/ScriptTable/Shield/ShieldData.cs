using UnityEngine;

[CreateAssetMenu(fileName = "ShieldData", menuName = "Scriptable Objects/ShieldData")]
public class ShieldData : ScriptableObject
{
    public Sprite shieldIcon;   // Hình ảnh icon khiên hiển thị trong UI
    public float duration = 5f; // Thời gian shield có hiệu lực (5 giây)
}


using UnityEngine;

public interface IInteractible
{
    void Destroy();
    void Rotate();
    void Glow( bool b);
    GameObject GetGameObject();
}

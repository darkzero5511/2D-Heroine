using System;
using UnityEngine;

[Serializable]
public class UI_TreeConnectDetails
{
    public UI_TreeConnectHandler childNode;
    public NodeDirectionType direction;
    [Range(150f, 300f)] public float length;
}

public class UI_TreeConnectHandler : MonoBehaviour
{
    private RectTransform rect;
    [SerializeField] private UI_TreeConnectDetails[] connectionDetails;
    [SerializeField] private UI_TreeConnection[] connections;

    private void OnValidate()
    {
        if (rect == null)
            rect = GetComponent<RectTransform>();

        if (connectionDetails.Length <= 0)
            return;

        if (connectionDetails.Length != connections.Length)
        {
            Debug.LogWarning("Amount of Details should be same as amount of connections. -" + gameObject.name);
            return;
        }

        UpdateConnections();
    }

    private void UpdateConnections()
    {
        for (int i = 0; i < connectionDetails.Length; i++)
        {
            var detail = connectionDetails[i];
            var connection = connections[i];
            Vector2 targetPosition = connection.GetConnectionPoint(rect);

            if (detail.childNode == null)
                continue;

            connection.DirectionConnection(detail.direction, detail.length);
            detail.childNode.SetPosition(targetPosition);
        }
    }

    public void SetPosition(Vector2 position)
        => rect.anchoredPosition = position;
}

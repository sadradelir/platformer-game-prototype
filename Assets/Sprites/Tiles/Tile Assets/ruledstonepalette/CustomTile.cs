using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Sadra/CustomTile")]
public class CustomTile : RuleTile<CustomTile.Neighbor>
{

    public class Neighbor : TilingRule.Neighbor
    {
        public const int Border = 3;
        public const int Inside = 4;
        public const int Nothing = 5;
    }

    public bool isInside;
    public bool isBorder;

    public override bool RuleMatch(int neighbor, TileBase other)
    {
        if (other is RuleOverrideTile)
            other = (other as RuleOverrideTile).m_InstanceTile;

        switch (neighbor)
        {
            case Neighbor.Nothing:
                return !other;
            case Neighbor.Border:
                return ((other is CustomTile) && (other as CustomTile).isBorder);
            case Neighbor.Inside:
                return ((other is CustomTile) && (other as CustomTile).isInside);
        }

        return base.RuleMatch(neighbor, other);
    }
}

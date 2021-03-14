using UnityEngine;

public class GridContainer : MonoBehaviour
{
    #region Properties

    [SerializeField] float padding = 50;
    [SerializeField] float margin = 50;
    [SerializeField] int row = 3;
    [SerializeField] int colum = 3;
    RectTransform rect;

    #endregion

    #region Methods

    public void Init(int row, int colum, float margin = 50f, float padding = 50f)
    {
        this.row = row;
        this.colum = colum;
        this.margin = margin;
        this.padding = padding;
    }

    public void CalculateChildernPosition(int row = 0, int colum = 0)
    {
        this.row = row;
        this.colum = colum;

        rect = GetComponent<RectTransform>();

        float parrentWidth = rect.rect.width;
        float parrentHeigth = rect.rect.height;

        float cellWidth = (parrentWidth - padding) / colum;
        float cellHeigth = (parrentHeigth - padding) / row;

        SetItemPos(cellWidth, cellHeigth);  
    }

    private void SetItemPos(float itemWidth, float itemHeigth)
    {
        Vector2 fristItemPos = new Vector2(
                -((rect.rect.width + padding - itemWidth) / 2) + padding, 
                 ((rect.rect.height +  padding - itemHeigth) / 2) - padding);

        float targetPosX = fristItemPos.x;
        float targetPosY = fristItemPos.y;  

        for (int i = 0; i < rect.childCount; i++)
        {
            var itemRect = rect.GetChild(i).GetComponent<RectTransform>();

            itemRect.sizeDelta = new Vector2(itemWidth - margin, itemHeigth - margin);
            itemRect.localScale = Vector2.one;

            if (i != 0)
            {
                targetPosX = i % colum == 0 ? fristItemPos.x : (targetPosX + itemWidth);
                targetPosY -= i % row == 0 ? itemHeigth : 0;
            }

            itemRect.anchoredPosition = new Vector2(targetPosX, targetPosY);
        }
    }

    #endregion
}

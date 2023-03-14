using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class małs : MonoBehaviour
{
    RawImage image;
    public Image pointer; 
    RenderScript render;
    public Slider range;
    public Slider strenght;
    // Start is called before the first frame update
    void Start()
    {
        image = gameObject.GetComponent<RawImage>();
        render = gameObject.GetComponent<RenderScript>();
    }

    // Update is called once per frame
    void Update()
    {
        setPointerSize(range.value);
        if (Input.GetKey(KeyCode.Mouse0))
        {
           // render.DrawCircle(500, 500, 1, 1);
            Vector2 mousePos = textCoords();
            render.DrawCircle((uint)mousePos.x, (uint)mousePos.y,(int)range.value,strenght.value);
        }
        float x = Input.mousePosition.x > (image.transform.position.x - image.rectTransform.rect.size.x / 2) ? Input.mousePosition.x : (image.transform.position.x - image.rectTransform.rect.size.x / 2);
        x = x < (image.transform.position.x + image.rectTransform.rect.size.x / 2) ? x : (image.transform.position.x + image.rectTransform.rect.size.x / 2);
        float y = Input.mousePosition.y > (image.transform.position.y - image.rectTransform.rect.size.y / 2) ? Input.mousePosition.y : (image.transform.position.y - image.rectTransform.rect.size.y / 2);
        y = y < (image.transform.position.y + image.rectTransform.rect.size.y / 2) ? y : (image.transform.position.y+ image.rectTransform.rect.size.y / 2);
        pointer.transform.position = new Vector3(x, y, pointer.transform.position.z);

    }

    Vector2 textCoords()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(image.rectTransform, Input.mousePosition, null, out Vector2 mousePos);

        //convert to texture pos
        mousePos.x += image.rectTransform.rect.size.x / 2;
        mousePos.y += image.rectTransform.rect.size.y / 2;


        mousePos.x *= render.TextSource.width / image.rectTransform.rect.size.x;
        mousePos.y *= render.TextSource.height / image.rectTransform.rect.size.y;

        return mousePos;
    }

    public void setPointerSize(float radius)
    {
        pointer.rectTransform.sizeDelta = new Vector2((radius*2 * image.rectTransform.rect.size.x / render.TextSource.width) , (radius*2 * image.rectTransform.rect.size.y / render.TextSource.height) );
    }
}

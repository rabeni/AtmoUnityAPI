using UnityEngine;
using System.Collections;

public class Strip : MonoBehaviour
{
    private Renderer[] _leds = new Renderer[144];
    [HideInInspector]
    public int _numLeds = 144;
    [SerializeField]
    private GameObject _led;

    void Awake()
    {
        Init();

        SetAll(new Color32(20, 0, 0, 255));
    }

    #region Public Methods

    //set index pixel's color
    public void setPixelColor(int index, Vector3 color)
    {
        Color32 c = new Color32((byte)color.x, (byte)color.y, (byte)color.z, 255);
        c = ReserveHeaderValue(c);

        if (index > (_numLeds - 1))
        {
            index = index - _numLeds - 1;
        }

        SetLedColor(index, c);
    }

    //set index pixel color
    public void setPixelColor(int index, Color32 color)
    {
        color = ReserveHeaderValue(color);

        if (index > (_numLeds - 1))
        {
            index = index - _numLeds - 1;
        }

        SetLedColor(index, color);
    }

    //set all pixel colors
    public void SetAll(Color32 color)
    {

        color = ReserveHeaderValue(color);

        for (int i = 0; i < _numLeds; i++)
        {
            setPixelColor(i, color);
        }
    }

    //set all pixel r values
    public void SetAllR(float r)
    {

        r = ReserveHeaderValue(r);

        for (int i = 0; i < _numLeds; i++)
        {
            SetLedColor(i, new Color(r, _leds[i].material.GetColor("_Color").g, _leds[i].material.GetColor("_Color").b));
        }
    }

    //set all pixel r values
    public void SetAllR(byte r)
    {

        r = ReserveHeaderValue(r);

        for (int i = 0; i < _numLeds; i++)
        {
            SetLedColor(i, new Color(r, _leds[i].material.GetColor("_Color").g, _leds[i].material.GetColor("_Color").b));
        }
    }

    //set all pixel g values
    public void SetAllG(float g)
    {

        g = ReserveHeaderValue(g);

        for (int i = 0; i < _numLeds; i++)
        {
            SetLedColor(i, new Color(_leds[i].material.GetColor("_Color").r, g, _leds[i].material.GetColor("_Color").b));
        }
    }

    //set all pixel g values
    public void SetAllG(byte g)
    {

        g = ReserveHeaderValue(g);

        for (int i = 0; i < _numLeds; i++)
        {
            SetLedColor(i, new Color(_leds[i].material.GetColor("_Color").r, g, _leds[i].material.GetColor("_Color").b));
        }
    }

    //set all pixel b values
    public void SetAllB(float b)
    {
        b = ReserveHeaderValue(b);

        for (int i = 0; i < _numLeds; i++)
        {
            SetLedColor(i, new Color(_leds[i].material.GetColor("_Color").r, _leds[i].material.GetColor("_Color").g, b));
        }
    }

    //set all pixel b values
    public void SetAllB(byte b)
    {
        b = ReserveHeaderValue(b);

        for (int i = 0; i < _numLeds; i++)
        {
            SetLedColor(i, new Color(_leds[i].material.GetColor("_Color").r, _leds[i].material.GetColor("_Color").g, b));
        }
    }

    public Color32 GetPixelColor(int index)
    {
        return GetLedColor(index);
    }

    #endregion



    #region Private methods

    // Instantiates leds on the scene
    private void Init()
    {
        int radius = 10;
        float rotateToTable = 149 * Mathf.Deg2Rad;
        int NumOfMissingLeds = 13;
        for (int i = 0; i < _numLeds; i++)
        {
            float angle = -i * Mathf.PI * 2 / (_numLeds + NumOfMissingLeds) + rotateToTable;
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius + new Vector3(0f, 0f, 10f);

            _leds[i] = (Instantiate(_led, pos, Quaternion.identity, gameObject.transform) as GameObject).GetComponent<Renderer>();
        }
    }

    //255 is reserved for serial communication header
    private Color32 ReserveHeaderValue(Color32 color)
    {
        if (color.r >= 255)
        {
            color.r = 244;
        }
        if (color.g >= 255)
        {
            color.g = 244;
        }
        if (color.b >= 255)
        {
            color.b = 244;
        }
        return color;
    }

    //255 is reserved for serial communication header
    private float ReserveHeaderValue(float color)
    {
        if (color >= 1)
        {
            color = 0.99f;
        }
        return color;
    }

    //255 is reserved for serial communication header
    private byte ReserveHeaderValue(byte color)
    {
        if (color >= 255)
        {
            color = 254;
        }
        return color;
    }

    private void SetLedColor(int index, Color32 color)
    {
        _leds[index].material.SetColor("_Color", color);
    }

    private Color32 GetLedColor(int index)
    {
        return _leds[index].material.GetColor("_Color");
    }

    public byte[] GetColorBytes()
    {
        int byteIndex = 0;

        byte[] byteValues = new byte[_numLeds * 3];

        lock (this)
        {
            foreach (Renderer led in _leds)
            {
                Color32 c = (Color32)led.material.color;
                byteValues[byteIndex++] = c.r;
                byteValues[byteIndex++] = c.g;
                byteValues[byteIndex++] = c.b;
            }
        }
        return byteValues;
    }

    #endregion
}



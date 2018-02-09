using UnityEngine;
using System.Collections;

public class Strip : MonoBehaviour
{
    private Renderer[] _leds = new Renderer[144];
    [HideInInspector]
    public int _numLeds = 144;
    public byte brightness = 0;
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

        if (index > (_numLeds - 1))
        {
            index = index - _numLeds - 1;
        }

        SetLedColor(index, c);
    }

    //set index pixel color
    public void setPixelColor(int index, Color32 color)
    {
        if (index > (_numLeds - 1))
        {
            index = index - _numLeds - 1;
        }

        SetLedColor(index, color);
    }

    //set all pixel colors
    public void SetAll(Color32 color)
    {
        for (int i = 0; i < _numLeds; i++)
        {
            setPixelColor(i, color);
        }
    }

    //set all pixel r values
    public void SetAllR(float r)
    {
        for (int i = 0; i < _numLeds; i++)
        {
            SetLedColor(i, new Color(r, _leds[i].material.GetColor("_Color").g, _leds[i].material.GetColor("_Color").b));
        }
    }

    //set all pixel r values
    public void SetAllR(byte r)
    {
        for (int i = 0; i < _numLeds; i++)
        {
            SetLedColor(i, new Color(r, _leds[i].material.GetColor("_Color").g, _leds[i].material.GetColor("_Color").b));
        }
    }

    //set all pixel g values
    public void SetAllG(float g)
    {
        for (int i = 0; i < _numLeds; i++)
        {
            SetLedColor(i, new Color(_leds[i].material.GetColor("_Color").r, g, _leds[i].material.GetColor("_Color").b));
        }
    }

    //set all pixel g values
    public void SetAllG(byte g)
    {
        for (int i = 0; i < _numLeds; i++)
        {
            SetLedColor(i, new Color(_leds[i].material.GetColor("_Color").r, g, _leds[i].material.GetColor("_Color").b));
        }
    }

    //set all pixel b values
    public void SetAllB(float b)
    {
        for (int i = 0; i < _numLeds; i++)
        {
            SetLedColor(i, new Color(_leds[i].material.GetColor("_Color").r, _leds[i].material.GetColor("_Color").g, b));
        }
    }

    //set all pixel b values
    public void SetAllB(byte b)
    {
        for (int i = 0; i < _numLeds; i++)
        {
            SetLedColor(i, new Color(_leds[i].material.GetColor("_Color").r, _leds[i].material.GetColor("_Color").g, b));
        }
    }

    public void SetBrightness(byte value)
    {
        brightness = value;
    }

    public void SetBrightness(float value)
    {
        brightness = (byte)(255 * value);
    }

    public Color32 GetPixelColor(int index)
    {
        return GetLedColor(index);
    }

    public byte[] GetColorBytes()
    {
        int byteIndex = 0;

        //+1 for brightness
        byte[] byteValues = new byte[_numLeds * 3 + 1];

        lock (this)
        {
            byteValues[byteIndex++] = MapByte(brightness);
            foreach (Renderer led in _leds)
            {
                Color32 c = (Color32)led.material.color;
                byteValues[byteIndex++] = MapByte(c.r);
                byteValues[byteIndex++] = MapByte(c.g);
                byteValues[byteIndex++] = MapByte(c.b);
            }
        }
        return byteValues;
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

    private void SetLedColor(int index, Color32 color)
    {
        _leds[index].material.SetColor("_Color", color);
    }

    private Color32 GetLedColor(int index)
    {
        return _leds[index].material.GetColor("_Color");
    }

    // Maps byte between 0-244. 255 is kept for header.
    private byte MapByte(byte b) 
    {
        if (b > 244)
            b = 244;

        return b;
    }

    #endregion
}



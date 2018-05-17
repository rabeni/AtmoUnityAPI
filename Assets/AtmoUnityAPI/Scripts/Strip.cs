/*
  Strip.cs - This script provides functions to control the AtmoLight strip.
  All public functions can be used to control the leds in different ways.
  There are 144 pixels to control which are represented by Led GameObjects in Unity.
  They can be seen if you zoom out in the Scene view.
  Created by Atmo, February 2, 2018.
*/

using UnityEngine;

public class Strip : MonoBehaviour
{
    public Strip instance;
    private Renderer[] _leds = new Renderer[144];
    [HideInInspector]
    public int _numLeds = 144;
    public byte brightness = 0;
    [SerializeField]
    private GameObject _led;


    void Awake()
    {
        //Check if instance already exists. 
        if (instance == null)
        {
            instance = this;

            Init();
            SetAll(new Color32(0, 0, 0, 255));
        }
    }

    #region Public Methods

    /// <summary>
    /// Sets index-th pixel to color. 
    /// </summary>
    /// <param name="index">Pixel index.</param>
    /// <param name="color">Pixel color. Parameters x,y,z stand for r,g,b.</param>
    public void SetPixelColor(int index, Vector3 color)
    {
        Color32 c = new Color32((byte)color.x, (byte)color.y, (byte)color.z, 255);

        if (index > (_numLeds - 1))
        {
            index = index - _numLeds - 1;
        }

        SetLedColor(index, c);
    }

    /// <summary>
    /// Sets index-th pixel to color. 
    /// </summary>
    /// <param name="index">Pixel index.</param>
    /// <param name="color">Pixel color. Parameter a, alpha is not used.</param>
    public void SetPixelColor(int index, Color32 color)
    {
        if (index > (_numLeds - 1))
        {
            index = index - _numLeds - 1;
        }

        SetLedColor(index, color);
    }

    /// <summary>
    /// Sets all pixel to color. 
    /// </summary>
    /// <param name="color">Pixel color. Parameter a, alpha is not used.</param>
    public void SetAll(Color32 color)
    {
        for (int i = 0; i < _numLeds; i++)
        {
            SetPixelColor(i, color);
        }
    }

    /// <summary>
    /// Sets r value of all pixels.
    /// </summary>
    /// <param name="r">Red.</param>
    public void SetAllR(float r)
    {
        for (int i = 0; i < _numLeds; i++)
        {
            SetLedColor(i, new Color(r, _leds[i].material.GetColor("_Color").g, _leds[i].material.GetColor("_Color").b));
        }
    }

    /// <summary>
    /// Sets r value of all pixels.
    /// </summary>
    /// <param name="r">Red.</param>
    public void SetAllR(byte r)
    {
        for (int i = 0; i < _numLeds; i++)
        {
            SetLedColor(i, new Color(r, _leds[i].material.GetColor("_Color").g, _leds[i].material.GetColor("_Color").b));
        }
    }

    /// <summary>
    /// Sets g value of all pixels.
    /// </summary>
    /// <param name="g">Green.</param>
    public void SetAllG(float g)
    {
        for (int i = 0; i < _numLeds; i++)
        {
            SetLedColor(i, new Color(_leds[i].material.GetColor("_Color").r, g, _leds[i].material.GetColor("_Color").b));
        }
    }

    /// <summary>
    /// Sets g value of all pixels.
    /// </summary>
    /// <param name="g">Green.</param>
    public void SetAllG(byte g)
    {
        for (int i = 0; i < _numLeds; i++)
        {
            SetLedColor(i, new Color(_leds[i].material.GetColor("_Color").r, g, _leds[i].material.GetColor("_Color").b));
        }
    }

    /// <summary>
    /// Sets b value of all pixels.
    /// </summary>
    /// <param name="b">Blue.</param>
    public void SetAllB(float b)
    {
        for (int i = 0; i < _numLeds; i++)
        {
            SetLedColor(i, new Color(_leds[i].material.GetColor("_Color").r, _leds[i].material.GetColor("_Color").g, b));
        }
    }

    /// <summary>
    /// Sets b value of all pixels.
    /// </summary>
    /// <param name="b">Blue.</param>
    public void SetAllB(byte b)
    {
        for (int i = 0; i < _numLeds; i++)
        {
            SetLedColor(i, new Color(_leds[i].material.GetColor("_Color").r, _leds[i].material.GetColor("_Color").g, b));
        }
    }

    /// <summary>
    /// Sets brightness value of all pixels.
    /// </summary>
    /// <param name="value">Brightness value (0-255).</param>
    public void SetBrightness(byte value)
    {
        brightness = value;
    }

    /// <summary>
    /// Sets brightness value of all pixels.
    /// </summary>
    /// <param name="value">Brightness value (0-1).</param>
    public void SetBrightness(float value)
    {
        brightness = (byte)(255 * value);
    }

    /// <summary>
    /// Gets color of index-th pixel.
    /// </summary>
    /// <param name="index">Pixel index.</param>
    /// <returns>Color.</returns>  
    public Color32 GetPixelColor(int index)
    {
        return GetLedColor(index);
    }

    /// <summary>
    /// Gets byte stream of brightness and all pixel colors mapped under 255 to maintain that for serial header.
    /// </summary>
    /// <returns>Byte stream of brightness and all pixel colors.</returns>
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
        int radius = 20;
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



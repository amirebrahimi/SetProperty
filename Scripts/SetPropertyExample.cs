// Copyright (c) 2014 Luminary LLC
// Licensed under The MIT License (See LICENSE for full text)
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetPropertyExample : MonoBehaviour 
{
	[System.Serializable]
	public class VanillaClass
	{
		public enum ExtraType
		{
			None,
			HotFudge,
			Mint,
		}

		[SerializeField, SetProperty("Extra")]
		private ExtraType extra;
		public ExtraType Extra
		{
			get
			{
				return extra;
			}
			set
			{
				extra = value;

				// For illustrative purposes
				if (value == ExtraType.None)
				{
					Debug.Log("Simple!");
				}
				else
				{
					Debug.Log ("Yummy!");
				}
			}
		}
	}

	[SerializeField, SetProperty("Number")]
	private float number;
	public float Number
	{
		get
		{
			return number;
		}
		private set
		{
			number = Mathf.Clamp01(value);
		}
	}

	[SerializeField, SetProperty("Text")]
	private string text;
	public string Text
	{
		get
		{
			return text;
		}
		set
		{
			text = value.ToUpper();
		}
	}

	[SerializeField, SetProperty("Sprite")]
    private Sprite sprite;
    public Sprite Sprite
    {
        get
        {
            return sprite;
        }
        set
        {
            sprite = value;
            Debug.Log($"The sprite {sprite.name} is a {sprite.texture.width}x{sprite.texture.height} image.");
        }
    }

	[SerializeField, SetProperty("RealNumbers")]
	private List<float> realNumbers;
	public List<float> RealNumbers { 
		get
		{
			return realNumbers;
		}
		set
		{
			for (int i = 0; i < value.Count; i++)
			{
				value[i] = Mathf.Clamp01(value[i]);
			}
			realNumbers = value;
		}
	}

	public VanillaClass vanilla;
}

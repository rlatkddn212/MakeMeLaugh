public enum UILayerType { Panel, PopUp, Menu };
public enum UIPriorityType { Backmost = -5000, Normal = 0, Foremost = +5000 };

public enum SpaceType { xyz, xy, xz };

public enum EaseType
{
	Linear,
	InSine, OutSine, InOutSine, OutInSine,

	InQuad, OutQuad, InOutQuad, OutInQuad,
	InCubic, OutCubic, InOutCubic, OutInCubic,
	InQuart, OutQuart, InOutQuart, OutInQuart,
	InQuint, OutQuint, InOutQuint, OutInQuint,

	InExpo, OutExpo, InOutExpo, OutInExpo,
	InCirc, OutCirc, InOutCirc, OutInCirc,

	InBounce, OutBounce, InOutBounce, OutInBounce,

	InElastic, OutElastic, InOutElastic, OutInElastic,

	InBack, OutBack, InOutBack, OutInBack,
	// Flash, InFlash, OutFlash, InOutFlash
}
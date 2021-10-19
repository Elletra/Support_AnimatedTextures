# Animated Textures #

This add-on adds support for animated textures.


## Using `Support_AnimatedTextures` ##

### <a name="models-textures-and-skins"></a>Models, Textures, and Skins ###

This add-on utilizes Torque's skin system in order to cycle through textures on a schedule.

First, you'll need to create a base texture. This is the fallback texture that your model uses. All it needs to be is just a blank image file.

Name your base texture `base.YOUR_TEXTURE_NAME_HERE.png`. Replace `YOUR_TEXTURE_NAME_HERE` with whatever you want your texture name to be.

Next, you need to add your frames.

Create however many frame textures you want, then name them like so: `frame000.YOUR_TEXTURE_NAME_HERE.png`, `frame001.YOUR_TEXTURE_NAME_HERE.png`, `frame002.YOUR_TEXTURE_NAME_HERE.png`, and so on.

**The frame numbers _must_ have leading zeros if they're under 100.**

Also, please be advised that **this add-on only supports up to 1,000 frames**, so you can only go up to `frame999`.

After this, put your model and your textures in the same folder.

**You need to add your frames to the resource manager manually.** To do this, use the [extraResources](https://github.com/qoh/bl-lib/blob/master/extraResources.cs) script in your code:

```js
addExtraResource("Add-Ons/My_AddOn/frame000.YOUR_TEXTURE_NAME_HERE.png");
addExtraResource("Add-Ons/My_AddOn/frame001.YOUR_TEXTURE_NAME_HERE.png");
addExtraResource("Add-Ons/My_AddOn/frame002.YOUR_TEXTURE_NAME_HERE.png");
addExtraResource("Add-Ons/My_AddOn/frame003.YOUR_TEXTURE_NAME_HERE.png");
// ...
```

The above code is just an example usage. Your code will probably look different.

Next step is using the API...

***

### <a name="api"></a>API ###

#### <a name="api-create-shape">`AnimTextures.createShape(className, data, framePrefix, numFrames, fps)` ####

Creates an object with an animated texture. The animation will start automatically.

If successful, it **returns the object.** If it fails to create an object, it **returns `0`** and prints an error.

| Argument | Type | Description |
| -------- | ---- | ----------- |
| className | class name | The class of object to create. **It must have an engine-defined `setSkinName()` method**, which is only found on descendants of the `ShapeBase` class. These are classes like `StaticShape`, `Player`, `Item`, `WheeledVehicle`, etc. |
| data | data block | The data block that the shape will have. |
| framePrefix | string | The prefix to all your frame texture names. For example, if you named your frames `frame000.YOUR_TEXTURE_NAME_HERE.png` and so on, you would put `frame` for this value. If you have the frames in a subfolder from where the model is, you would include the subfolder: `folder/frame`.
| numFrames | integer | The number of frames your animation has. |
| fps | integer | The framerate you want your animation to have. |

##

#### <a name="api-add-shape">`AnimTextures.addShape(shape)` ####

Adds a shape to the shape set.

**Returns**  [`AnimTexturesError`](#api-error-handling).

| Argument | Type | Description |
| -------- | ---- | ----------- |
| shape | `ShapeBase` | The object to add to the shape set |

##

#### <a name="api-remove-shape">`AnimTextures.removeShape(shape)` ####

Removes a shape from shape set and stops its animation.

| Argument | Type | Description |
| -------- | ---- | ----------- |
| shape | `ShapeBase` | The shape to remove from the shape set |

##

#### <a name="api-has-shape">`AnimTextures.hasShape(shape)` ####

Checks whether a shape is in the shape set.

**Returns** `boolean`.

| Argument | Type | Description |
| -------- | ---- | ----------- |
| shape | `ShapeBase` | The shape to check |

##

#### <a name="api-validate-num-frames">`AnimTextures.validateNumFrames(numFrames)` ####

Validates the number of frames your animation has.

**Returns**  [`AnimTexturesError`](#api-error-handling).

| Argument | Type | Description |
| -------- | ---- | ----------- |
| numFrames | integer | The number of frames your animation has. |

##

#### <a name="api-validate-num-fps">`AnimTextures.validateFPS(fps)` ####

Validates the framerate your animation has.

**Returns**  [`AnimTexturesError`](#api-error-handling).

| Argument | Type | Description |
| -------- | ---- | ----------- |
| fps | integer | The framerate your animation has. |

##

#### <a name="api-print-error">`AnimTextures.printError(error)` ####

Prints an error message based on the error passed in.

| Argument | Type | Description |
| -------- | ---- | ----------- |
| error | [`AnimTexturesError`](#api-error-handling) | The error you want to print a message for. |

##

#### <a name="api-check-can-create-shape">`AnimTextures.checkCanCreateShape(className, data, numFrames, fps)` ####

Checks whether a shape with the data specified can be created.

[`AnimTextures::createShape`](#api-create-shape) already performs this check, so you don't have to call it beforehand. However, [`AnimTextures::createShape`](#api-create-shape) does print an error if the check fails, so if you don't want that to happen, you'll have to perform the check before calling it.

**Returns**  [`AnimTexturesError`](#api-error-handling).

| Argument | Type | Description |
| -------- | ---- | ----------- |
| className | class name | The class of object you want to create. **It must have an engine-defined `setSkinName()` method**, which is only found on descendants of the `ShapeBase` class. These are classes like `StaticShape`, `Player`, `Item`, `WheeledVehicle`, etc. |
| data | data block | The data block you want your shape to have. |
| numFrames | integer | The number of frames your animation has. |
| fps | integer | The framerate you want your animation to have. |

##

#### <a name="api-check-can-add-shape">`AnimTextures.checkCanAddShape(className, numFrames, fps)` ####

Checks whether a shape with the data specified can be added to the shape set.

[`AnimTextures::addShape`](#api-add-shape) already performs this check, so you don't need to call it beforehand.

**Returns**  [`AnimTexturesError`](#api-error-handling).

| Argument | Type | Description |
| -------- | ---- | ----------- |
| className | class name | The class of object you want to create. **It must have an engine-defined `setSkinName()` method**, which is only found on descendants of the `ShapeBase` class. These are classes like `StaticShape`, `Player`, `Item`, `WheeledVehicle`, etc. |
| numFrames | integer | The number of frames your animation has. |
| fps | integer | The framerate you want your animation to have. |

##

#### <a name="api-anim-tex-start">`ShapeBase::animTex_start()`

Starts a shape's texture animation, if it can.

**Returns**  [`AnimTexturesError`](#api-error-handling).

##

#### <a name="api-anim-tex-stop">`ShapeBase::animTex_stop()`

Stops a shape's texture animation.

##

#### <a name="api-anim-tex-check-can-start">`ShapeBase::animTex_checkCanStart()`

Checks whether a shape's texture animation is valid.

[`ShapeBase::animTex_start`](#api-anim-tex-start) already performs this check, so you don't need to call it beforehand.

**Returns**  [`AnimTexturesError`](#api-error-handling).

##

#### <a name="api-anim-tex-set-num-frames">`ShapeBase::animTex_setNumFrames(numFrames)`

Sets the number of frames you want your shape's texture animation to have, if valid.

**Returns**  [`AnimTexturesError`](#api-error-handling).

| Argument | Type | Description |
| -------- | ---- | ----------- |
| numFrames | integer | The number of frames you want your texture animation to have. |

##

#### <a name="api-anim-tex-set-fps">`ShapeBase::animTex_setFPS(fps)`

Sets the framerate you want your shape's texture animation to have, if valid.

**Returns**  [`AnimTexturesError`](#api-error-handling).

| Argument | Type | Description |
| -------- | ---- | ----------- |
| fps | integer | The framerate you want your texture animation to have. |

##

#### <a name="api-anim-tex-set-prefix">`ShapeBase::animTex_setPrefix(framePrefix)`

Sets the frame prefix you want your shape's texture animation to have.

| Argument | Type | Description |
| -------- | ---- | ----------- |
| framePrefix | string | The frame prefix you want your texture animation to have. |

##

#### <a name="api-anim-tex-get-num-frames">`ShapeBase::animTex_getNumFrames()`

Gets the number of frames your shape's texture animation has.

**Returns** `integer`.

##

#### <a name="api-anim-tex-get-fps">`ShapeBase::animTex_getFPS()`

Gets the framerate of your shape's texture animation.

**Returns** `integer`.

##

#### <a name="api-anim-tex-get-prefix">`ShapeBase::animTex_getPrefix()`

Gets the frame prefix of your shape's texture animation.

**Returns** `string`.

##

#### <a name="api-anim-tex-get-curr-frame">`ShapeBase::animTex_getCurrFrame()`

Gets the current frame your shape's texture animation is on.

**Returns** `integer`.

***

#### <a name="api-error-handling">Error Handling ####

Some functions return an `AnimTexturesError`, which will be one of the following:

| Variable | Description |
| -------- | ----------- |
| $AnimTextures::Error::None | There was no error and the operation was successful. |
| $AnimTextures::Error::ClassName | The class name specified does not support texture skins. |
| $AnimTextures::Error::DataBlock | The data block specified does not exist. |
| $AnimTextures::Error::ShapeLimit | The limit for shapes with animated textures has been reached. |
| $AnimTextures::Error::NotInSet | The shape you're trying to animate has not been added to the shape set. Either create a new shape with `AnimTextures.createShape()` or add your shape to the set with `AnimTextures.addShape()`. |
| $AnimTextures::Error::MinFrames | The number of frames specified is too low. |
| $AnimTextures::Error::MaxFrames | The number of frames specified is too high. |
| $AnimTextures::Error::MinFPS | The framerate specified is too low. |
| $AnimTextures::Error::MaxFPS | The framerate specified is too high. |

***

#### <a name="api-constants">Constants ####

There are some constants available for this add-on. You may use them, but ***do not change these under any circumstances.***

| Variable | Value | Description |
| -------- | ----- | ----------- |
| $AnimTextures::Version | 1 | The version of the add-on that is installed. |
| $AnimTextures::MaxShapes | 150 | The maximum number of shapes with animated textures that are allowed. After about 150, things start to glitch out and get laggy. |
| $AnimTextures::MinFrames | 1 | The minimum number of frames an animation can have. |
| $AnimTextures::MaxFrames | 1000 | The maximum number of frames an animation can have. |
| $AnimTextures::MinFPS | 1 | The lowest framerate an animation can have. |
| $AnimTextures::MaxFPS | 1000 | The highest framerate an animation can have. |

As you may have realized, there are very logical reasons for all these values so ***you should not change them.***

***

### <a name="examples">Examples ###

#### <a name="examples-basic-usage">Basic Usage ####

```js
// The static shape datablock
datablock StaticShapeData (MyStaticShapeData)
{
	shapeFile = "Add-Ons/My_AddOn/MyShape.dts";
};

// First, you need to initialize the animation by adding the frames to the resource manager
function initMyAddOn ()  // Just an example -- Don't actually name your function this.
{
	//* Add frames to the resource manager *//

	addExtraResource("Add-Ons/My_AddOn/frame000.myTexture.png");
	addExtraResource("Add-Ons/My_AddOn/frame001.myTexture.png");
	addExtraResource("Add-Ons/My_AddOn/frame002.myTexture.png");
	addExtraResource("Add-Ons/My_AddOn/frame003.myTexture.png");
	addExtraResource("Add-Ons/My_AddOn/frame004.myTexture.png");
	addExtraResource("Add-Ons/My_AddOn/frame005.myTexture.png");
	addExtraResource("Add-Ons/My_AddOn/frame006.myTexture.png");
	addExtraResource("Add-Ons/My_AddOn/frame007.myTexture.png");
}

// Make sure you call the function before the mission is created!
initMyAddOn();

// Here's an example function for the basic creation of a shape:
function exampleFunction (%position)
{
	%shape = AnimTextures.createShape(StaticShape, MyDataBlock, "frame", 8, 60);

	if ( isObject (%shape) )
	{
		%shape.setTransform(%position);
	}

	return %shape;
}
```

##

#### <a name="examples-frames-subfolder">Keeping the Frames in a Subfolder ####

```js
datablock StaticShapeData (MyStaticShapeData)
{
	// In this example, we keep our model in a folder named `assets`
	shapeFile = "Add-Ons/My_AddOn/assets/MyShape.dts";
};

function initMyAddOn ()
{
	//* In this example, we keep our frames in a subfolder named `myFramesFolder`, which is a subfolder
	//  from where the model is *//

	addExtraResource("Add-Ons/My_AddOn/assets/myFramesFolder/frame000.myTexture.png");
	addExtraResource("Add-Ons/My_AddOn/assets/myFramesFolder/frame001.myTexture.png");
	addExtraResource("Add-Ons/My_AddOn/assets/myFramesFolder/frame002.myTexture.png");
	addExtraResource("Add-Ons/My_AddOn/assets/myFramesFolder/frame003.myTexture.png");
	addExtraResource("Add-Ons/My_AddOn/assets/myFramesFolder/frame004.myTexture.png");
	addExtraResource("Add-Ons/My_AddOn/assets/myFramesFolder/frame005.myTexture.png");
	addExtraResource("Add-Ons/My_AddOn/assets/myFramesFolder/frame006.myTexture.png");
	addExtraResource("Add-Ons/My_AddOn/assets/myFramesFolder/frame007.myTexture.png");
}

// ...

function exampleFunction (%position)
{
	// Since we're keeping our frames in a subfolder named `myFramesFolder`, we need to include it
	// in the frame prefix
	%shape = AnimTextures.createShape(StaticShape, MyDataBlock, "myFramesFolder/frame", 8, 60);

	// ...
}
```

##

#### <a name="examples-different-prefix">Having a Frame Prefix Other than `frame` ####

You don't need to have all your frames prefixed with `frame###`. That's just a convention!

You can prefix them with whatever, as long as they're numbered properly.

```js
function initMyAddOn ()
{
	//* In this example, we have our frames prefixed with `water` *//

	addExtraResource("Add-Ons/My_AddOn/water000.myTexture.png");
	addExtraResource("Add-Ons/My_AddOn/water001.myTexture.png");
	addExtraResource("Add-Ons/My_AddOn/water002.myTexture.png");
	addExtraResource("Add-Ons/My_AddOn/water003.myTexture.png");
	addExtraResource("Add-Ons/My_AddOn/water004.myTexture.png");
}

// ...

function exampleFunction (%position)
{
	// In this example, we have our frames prefixed with `water`
	%shape = AnimTextures.createShape(StaticShape, MyDataBlock, "water", 5, 60);

	// ...
}
```

##

#### <a name="examples-add-frames-with-loop">Adding Frames with a Loop ####

Calling `addExtraResource()` manually for each and every frame is not good.

Here's a better way of adding them:

```js
function initMyAddOn ()
{
	%numFrames = 64;

	for (%i = 0; %i < %numFrames; %i++)
	{
		%frame = %i;

		//* Add leading zeros *//

		if (%i < 10)
		{
			%frame = "0" @ %frame;
		}

		if (%i < 100)
		{
			%frame = "0" @ %frame;
		}

		// Add frames to the resource manager
		addExtraResource("Add-Ons/My_AddOn/frame" @ %frame @ ".myTexture.png");
	}
}
```

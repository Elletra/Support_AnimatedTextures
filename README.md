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

#### <a name="api-create-shape">`AnimTextures.createShape(className, data, namePrefix, numFrames, fps)` ####

Creates an object with an animated texture. The animation will start automatically.

If successful, it **returns the object.** If it fails to create an object, it **returns `0`** and prints an error.

| Argument | Type | Description |
| -------- | ---- | ----------- |
| className | class name | The class of object to create. **It must have an engine-defined `setSkinName()` method**, which is only found on descendants of the `ShapeBase` class. These are classes like `StaticShape`, `Player`, `Item`, `WheeledVehicle`, etc. |
| data | data block | The data block that the shape will have. |
| namePrefix | string | The prefix to all your frame texture names. For example, if you named your frames `frame000.YOUR_TEXTURE_NAME_HERE.png` and so on, you would put `frame` for this value. If you have the frames in a subfolder, you would include the subfolder: `subfolder/frame`.
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
| shape | `ShapeBase` | The shape to add to the shape set |

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

#### <a name="api-start-texture-anim">`ShapeBase::startTextureAnim()`

Starts a shape's texture animation, if it can.

**Returns**  [`AnimTexturesError`](#api-error-handling).

##

#### <a name="api-stop-texture-anim">`ShapeBase::stopTextureAnim()`

Stops a shape's texture animation.

##

#### <a name="api-check-can-anim-texture">`ShapeBase::checkCanAnimTexture()`

Checks whether a shape's texture animation is valid.

[`ShapeBase::startTextureAnim`](#api-start-texture-anim) already performs this check, so you don't need to call it beforehand.

**Returns**  [`AnimTexturesError`](#api-error-handling).

##

#### <a name="api-set-anim-texture-frames">`ShapeBase::setAnimTextureFrames(numFrames)`

Sets the number of frames you want your shape's texture animation to have, if valid.

**Returns**  [`AnimTexturesError`](#api-error-handling).

| Argument | Type | Description |
| -------- | ---- | ----------- |
| numFrames | integer | The number of frames you want your texture animation to have. |

##

#### <a name="api-set-anim-texture-fps">`ShapeBase::setAnimTextureFPS(fps)`

Sets the framerate you want your shape's texture animation to have, if valid.

**Returns**  [`AnimTexturesError`](#api-error-handling).

| Argument | Type | Description |
| -------- | ---- | ----------- |
| fps | integer | The framerate you want your texture animation to have. |

##

#### <a name="api-set-anim-texture-prefix">`ShapeBase::setAnimTexturePrefix(namePrefix)`

Sets the name prefix you want your shape's texture animation to have.

| Argument | Type | Description |
| -------- | ---- | ----------- |
| namePrefix | string | The name prefix you want your texture animation to have. |

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


### Example Usage ###

```js
// First, you need to initialize the animation by adding the frames to the resource manager.
function initMyAddOn ()  // Just an example -- Don't actually name your function this.
{
	for (%i = 0; %i < 12; %i++)
	{
		%frame = %i;

		//* Add trailing zeros *//

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

// Make sure you call the function before the mission is created!
initMyAddOn();

// Here's an example function for the basic creation of a shape:
function createMyShape (%position)  // Just an example -- Don't actually name your function this.
{
	%shape = AnimTextures.createShape(StaticShape, MyDataBlock, "frame", 12, 60);

	if ( !isObject (%shape) )
	{
		%shape.setTransform(%position);
	}

	return %shape;
}
```

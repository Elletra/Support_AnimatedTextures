# Animated Textures #

This mod adds support for animated textures.


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
addExtraResource("Add-Ons/AnimTexture_MyAddOn/assets/textures/frame000.YOUR_TEXTURE_NAME_HERE.png");
addExtraResource("Add-Ons/AnimTexture_MyAddOn/assets/textures/frame001.YOUR_TEXTURE_NAME_HERE.png");
addExtraResource("Add-Ons/AnimTexture_MyAddOn/assets/textures/frame002.YOUR_TEXTURE_NAME_HERE.png");
addExtraResource("Add-Ons/AnimTexture_MyAddOn/assets/textures/frame003.YOUR_TEXTURE_NAME_HERE.png");
// ...
```

The above code is just an example usage. Your code will probably look different.

Next step is using the API...

***

### <a name="api"></a>API ###

#### <a name="api-create-shape">`AnimTextures.createShape(data, namePrefix, numFrames, fps)` ####

Creates a `StaticShape` with an animated texture. The animation will start automatically.

If successful, it **returns the shape.** If it fails to create a shape, it **returns `0`** and prints an error.

| Argument | Type | Description |
| -------- | ---- | ----------- |
| data | `StaticShapeData` | The datablock that the shape will have. |
| namePrefix | string | The prefix to all your frame texture names. For example, if you named your frames `frame000.YOUR_TEXTURE_NAME_HERE.png` and so on, you would put `frame` for this value. If you have the frames in a subfolder, you would include the subfolder: `subfolder/frame`.
| numFrames | integer | The number of frames your animation has. |
| fps | integer | The framerate you want your animation to have. |

##

#### <a name="api-add-shape">`AnimTextures.addShape(shape)` ####

Adds a shape to the static shape set.

**Returns**  [`AnimTexturesError`](#api-error-handling).

| Argument | Type | Description |
| -------- | ---- | ----------- |
| shape | `StaticShape` | The static shape to add to the shape set |

##

#### <a name="api-remove-shape">`AnimTextures.removeShape(shape)` ####

Removes a shape from static shape set and stops its animation.

| Argument | Type | Description |
| -------- | ---- | ----------- |
| shape | `StaticShape` | The static shape to remove from the shape set |

##

#### <a name="api-has-shape">`AnimTextures.hasShape(shape)` ####

Checks whether a shape is in the static shape set.

**Returns** `boolean`.

| Argument | Type | Description |
| -------- | ---- | ----------- |
| shape | `StaticShape` | The static shape to add to the shape set |

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

#### <a name="api-check-can-create-shape">`AnimTextures.checkCanCreateShape(numFrames)` ####

Checks whether a static shape with an animated texture can be created.

**Returns**  [`AnimTexturesError`](#api-error-handling).

| Argument | Type | Description |
| -------- | ---- | ----------- |
| numFrames | integer | The number of frames your animation has. |

##

#### <a name="api-print-error">`AnimTextures.printError(error)` ####

Prints an error message based on the error passed in.

| Argument | Type | Description |
| -------- | ---- | ----------- |
| error | [`AnimTexturesError`](#api-error-handling) | The error you want to print a message for. |

##

#### <a name="api-check-can-anim-texture">`StaticShape::checkCanAnimTexture()`

Checks whether a static shape's texture animation is valid.

**Returns**  [`AnimTexturesError`](#api-error-handling).

##

#### <a name="api-start-texture-anim">`StaticShape::startTextureAnim()`

Starts a static shape's texture animation.

**Returns**  [`AnimTexturesError`](#api-error-handling).

##

#### <a name="api-stop-texture-anim">`StaticShape::stopTextureAnim()`

Stops a static shape's texture animation.

##

#### <a name="api-set-anim-texture-frames">`StaticShape::setAnimTextureFrames(numFrames)`

Sets the number of frames you want your static shape's texture animation to have, if valid.

**Returns**  [`AnimTexturesError`](#api-error-handling).

| Argument | Type | Description |
| -------- | ---- | ----------- |
| numFrames | integer | The number of frames you want your texture animation to have. |

##

#### <a name="api-set-anim-texture-fps">`StaticShape::setAnimTextureFPS(fps)`

Sets the framerate you want your static shape's texture animation to have, if valid.

**Returns**  [`AnimTexturesError`](#api-error-handling).

| Argument | Type | Description |
| -------- | ---- | ----------- |
| fps | integer | The framerate you want your texture animation to have. |

##

#### <a name="api-set-anim-texture-prefix">`StaticShape::setAnimTexturePrefix(namePrefix)`

Sets the name prefix you want your static shape's texture animation to have.

| Argument | Type | Description |
| -------- | ---- | ----------- |
| namePrefix | string | The name prefix you want your texture animation to have. |

***

#### <a name="api-error-handling">Error Handling ####

Some functions return an `AnimTexturesError`, which will be one of the following:

| Variable | Description |
| -------- | ----------- |
| $AnimTextures::Error::None | There was no error and the operation was successful. |
| $AnimTextures::Error::ShapeLimit | The limit for static shapes with animated textures has been reached. |
| $AnimTextures::Error::NotInSet | The static shape you're trying to animate has not been added to the shape set. Either create a new static shape with `AnimTextures.createShape()` or add your shape to the set with `AnimTextures.addShape()`. |
| $AnimTextures::Error::MinFrames | The number of frames specified is too low. |
| $AnimTextures::Error::MaxFrames | The number of frames specified is too high. |
| $AnimTextures::Error::MinFPS | The framerate specified is too low. |
| $AnimTextures::Error::MaxFPS | The framerate specified is too high. |

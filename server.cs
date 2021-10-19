function AnimTextures::onAdd ( %this )
{
	// Use `addShape()`, `removeShape()`, and `hasShape()` -- Do NOT use this field directly!!
	%this._animTexShapes = new SimSet ();
}

function AnimTextures::onRemove ( %this )
{
	%this._animTexShapes.deleteAll ();
	%this._animTexShapes.delete ();
}

function AnimTextures::createShape ( %this, %className, %data, %framePrefix, %numFrames, %fps )
{
	if ( %className $= "" )
	{
		%className = StaticShape;
	}

	%error = %this.checkCanCreateShape (%className, %data, %numFrames, %fps);

	if ( %error != $AnimTextures::Error::None )
	{
		%this.printError (%error);
		return 0;
	}

	%shape = new (%className) ()
	{
		dataBlock = %data;

		// These are all private -- Use their respective getters and setters to interact with them.
		_anim_framePrefix = %framePrefix;
		_anim_numFrames = %numFrames;
		_anim_fps = %fps;
		_anim_currFrame = 0;
	};

	MissionCleanup.add (%shape);
	AnimTextures._animTexShapes.add (%shape);

	%error = %shape.animTex_start ();

	if ( %error != $AnimTextures::Error::None )
	{
		%shape.delete ();
		%this.printError (%error);
		return 0;
	}

	return %shape;
}

function AnimTextures::addShape ( %this, %shape )
{
	if ( !%this.hasShape (%shape) )
	{
		%error = %this.checkCanAddShape (%shape.getClassName (), %shape._anim_numFrames, %shape._anim_fps);

		if ( %error != $AnimTextures::Error::None )
		{
			return %error;
		}

		%this._animTexShapes.add (%shape);
	}

	return %shape.animTex_start ();
}

function AnimTextures::removeShape ( %this, %shape )
{
	if ( %this.hasShape (%shape) )
	{
		%shape.animTex_stop ();
		%this._animTexShapes.remove (%shape);
	}
}

function AnimTextures::hasShape ( %this, %shape )
{
	return %this._animTexShapes.isMember (%shape);
}

function AnimTextures::getShapeCount ( %this )
{
	return %this._animTexShapes.getCount ();
}

function AnimTextures::getShape ( %this, %index )
{
	return %this._animTexShapes.getObject (%index);
}

function AnimTextures::validateNumFrames ( %this, %numFrames )
{
	if ( %numFrames $= "" || %numFrames < $AnimTextures::MinFrames )
	{
		return $AnimTextures::Error::MinFrames;
	}

	if ( %numFrames > $AnimTextures::MaxFrames )
	{
		return $AnimTextures::Error::MaxFrames;
	}

	return $AnimTextures::Error::None;
}

function AnimTextures::validateFPS ( %this, %fps )
{
	if ( %fps $= "" || %fps < $AnimTextures::MinFPS )
	{
		return $AnimTextures::Error::MinFPS;
	}

	if ( %fps > $AnimTextures::MaxFPS )
	{
		return $AnimTextures::Error::MaxFPS;
	}

	return $AnimTextures::Error::None;
}

function AnimTextures::printError ( %this, %error )
{
	switch ( %error )
	{
		case $AnimTextures::Error::ClassName:
			error ("ERROR: The class name specified does not support texture skins");

		case $AnimTextures::Error::DataBlock:
			error ("ERROR: The data block specified does not exist");

		case $AnimTextures::Error::ShapeLimit:
			error ("ERROR: Cannot have more than " @ $AnimTextures::MaxShapes @ " shape(s) with animated textures");

		case $AnimTextures::Error::NotInSet:
			error ("ERROR: Shape is not in animated texture set");

		case $AnimTextures::Error::MinFrames:
			error ("ERROR: Animated textures must have at least " @ $AnimTextures::MinFrames @ " frame(s)");

		case $AnimTextures::Error::MaxFrames:
			error ("ERROR: Animated textures cannot have more than " @ $AnimTextures::MaxFrames @ " frame(s)");

		case $AnimTextures::Error::MinFPS:
			error ("ERROR: Animated textures must have a framerate of at least " @ $AnimTextures::MinFPS);

		case $AnimTextures::Error::MaxFPS:
			error ("ERROR: Animated textures cannot have a higher framerate than " @ $AnimTextures::MaxFPS);
	}
}

function AnimTextures::checkCanCreateShape ( %this, %className, %data, %numFrames, %fps )
{
	%error = %this.checkCanAddShape (%className, %numFrames, %fps);

	if ( %error != $AnimTextures::Error::None && !isObject (%data) )
	{
		return $AnimTextures::Error::DataBlock;
	}

	return %error;
}

function AnimTextures::checkCanAddShape ( %this, %className, %numFrames, %fps )
{
	if ( !isFunction (%className, "setSkinName") )
	{
		return $AnimTextures::Error::ClassName;
	}

	if ( %this._animTexShapes.getCount () >= $AnimTextures::MaxShapes )
	{
		return $AnimTextures::Error::ShapeLimit;
	}

	%error = %this.validateNumFrames (%numFrames);

	if ( %error != $AnimTextures::Error::None )
	{
		return %error;
	}

	return %this.validateFPS (%fps);
}

// ----------------------------------------------------------------


package Support_AnimatedTextures
{
	// We package custom functions so they disappear once the server closes/restarts.
	function AnimTextures_init ()
	{
		// !!! THESE VARIABLES ARE CONSTANTS !!!
		//
		// They are NOT prefs!!
		//
		// You can use them, but do NOT change them!

		$AnimTextures::Version = 1;

		$AnimTextures::MaxShapes = 150;
		$AnimTextures::MinFrames = 1;
		$AnimTextures::MaxFrames = 1000;
		$AnimTextures::MinFPS = 1;
		$AnimTextures::MaxFPS = 1000;

		%error = -1;

		$AnimTextures::Error::None = %error++;
		$AnimTextures::Error::ClassName = %error++;
		$AnimTextures::Error::DataBlock = %error++;
		$AnimTextures::Error::ShapeLimit = %error++;
		$AnimTextures::Error::NotInSet = %error++;
		$AnimTextures::Error::MinFrames = %error++;
		$AnimTextures::Error::MaxFrames = %error++;
		$AnimTextures::Error::MinFPS = %error++;
		$AnimTextures::Error::MaxFPS = %error++;

		if ( !isObject (AnimTextures) )
		{
			MissionCleanup.add (new ScriptObject (AnimTextures));
		}
	}

	function ShapeBase::animTex_start ( %this )
	{
		%error = %this.animTex_checkCanStart ();

		if ( %error != $AnimTextures::Error::None )
		{
			return %error;
		}

		%this._animTextureLoop ();

		return $AnimTextures::Error::None;
	}

	function ShapeBase::animTex_stop ( %this )
	{
		cancel (%this._anim_loop);
	}

	function ShapeBase::animTex_setNumFrames ( %this, %numFrames )
	{
		%error = AnimTextures.validateNumFrames (%numFrames);

		if ( %error == $AnimTextures::Error::None )
		{
			%this._anim_numFrames = %numFrames;
		}

		return %error;
	}

	function ShapeBase::animTex_setFPS ( %this, %fps )
	{
		%error = AnimTextures.validateFPS (%fps);

		if ( %error == $AnimTextures::Error::None )
		{
			%this._anim_fps = %fps;
		}

		return %error;
	}

	function ShapeBase::animTex_setPrefix ( %this, %framePrefix )
	{
		%this._anim_framePrefix = %framePrefix;
	}

	function ShapeBase::animTex_getNumFrames ( %this )
	{
		return %this._anim_numFrames;
	}

	function ShapeBase::animTex_getFPS ( %this )
	{
		return %this._anim_fps;
	}

	function ShapeBase::animTex_getPrefix ( %this )
	{
		return %this._anim_framePrefix;
	}

	function ShapeBase::animTex_getCurrFrame ( %this )
	{
		return %this._anim_currFrame;
	}

	function ShapeBase::animTex_checkCanStart ( %this )
	{
		%error = AnimTextures.validateNumFrames (%this._anim_numFrames);

		if ( %error != $AnimTextures::Error::None )
		{
			return %error;
		}

		if ( !AnimTextures.hasShape (%this) )
		{
			return $AnimTextures::Error::NotInSet;
		}

		return $AnimTextures::Error::None;
	}

	// Private method -- Do NOT call this!
	function ShapeBase::_animTextureLoop ( %this )
	{
		cancel (%this._anim_loop);

		%frame = %this._anim_currFrame;

		if ( %frame < 10 )
		{
			%frame = "0" @ %frame;
		}

		if ( %frame < 100 )
		{
			%frame = "0" @ %frame;
		}

		%this.setSkinName (%this._anim_framePrefix @ %frame);
		%this._anim_currFrame++;

		if ( %this._anim_currFrame >= %this._anim_numFrames )
		{
			%this._anim_currFrame = 0;
		}

		%this._anim_loop = %this.schedule (1000 / %this._anim_fps, "_animTextureLoop");
	}

	function createMission ()
	{
		Parent::createMission ();
		AnimTextures_init ();
	}

	function destroyServer ()
	{
		// Clean up after ourselves.
		deleteVariables ("$AnimTextures::*");
		Parent::destroyServer ();
	}
};
activatePackage (Support_AnimatedTextures);

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

function AnimTextures::createShape ( %this, %data, %namePrefix, %numFrames, %fps )
{
	%error = %this.checkCanCreateShape (%numFrames, %fps);

	if ( %error != $AnimTextures::Error::None )
	{
		%this.printError (%error);
		return 0;
	}

	%shape = new StaticShape ()
	{
		dataBlock = %data;
		anim_currFrame = 0;
		anim_namePrefix = %namePrefix;
	};

	MissionCleanup.add (%shape);
	AnimTextures._animTexShapes.add (%shape);

	%shape.anim_numFrames = %numFrames;
	%shape.anim_fps = (%fps $= "" ? $AnimTextures::DefaultFPS : %fps); 

	%error = %shape.startAnimTextureLoop ();

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
		%error = %this.checkCanCreateShape (%shape.anim_numFrames, %shape.anim_fps);

		if ( %error != $AnimTextures::Error::None )
		{
			return %error;
		}

		%this._animTexShapes.add (%shape);
	}

	return %shape.startAnimTextureLoop ();
}

function AnimTextures::removeShape ( %this, %shape )
{
	if ( %this.hasShape (%shape) )
	{
		%shape.stopAnimTextureLoop ();
		%this._animTexShapes.remove (%shape);
	}
}

function AnimTextures::hasShape ( %this, %shape )
{
	return %this._animTexShapes.isMember (%shape);
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

function AnimTextures::validateFramerate ( %this, %fps )
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

function AnimTextures::checkCanCreateShape ( %this, %numFrames, %fps )
{
	if ( %this._animTexShapes.getCount () >= $AnimTextures::MaxShapes )
	{
		return $AnimTextures::Error::ShapeLimit;
	}

	%error = %this.validateNumFrames (%numFrames);

	if ( %error != $AnimTextures::Error::None )
	{
		return %error;
	}

	return %this.validateFramerate (%fps);
}

function AnimTextures::printError ( %this, %error )
{
	switch ( %error )
	{
		case $AnimTextures::Error::ShapeLimit:
			error ("ERROR: Animated texture shape limit reached");

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

		$AnimTextures::MaxShapes = 100;
		$AnimTextures::MinFrames = 2;
		$AnimTextures::MaxFrames = 1000;
		$AnimTextures::MinFPS = 1;
		$AnimTextures::MaxFPS = 1000;
		$AnimTextures::DefaultFPS = 30;

		$AnimTextures::Error::None = 0;
		$AnimTextures::Error::ShapeLimit = 1;
		$AnimTextures::Error::NotInSet = 2;
		$AnimTextures::Error::MinFrames = 3;
		$AnimTextures::Error::MaxFrames = 4;
		$AnimTextures::Error::MinFPS = 5;
		$AnimTextures::Error::MaxFPS = 6;

		if ( !isObject (AnimTextures) )
		{
			MissionCleanup.add (new ScriptObject (AnimTextures));
		}
	}

	function StaticShape::startAnimTextureLoop ( %this )
	{
		%error = %this.checkCanAnimate ();

		if ( %error != $AnimTextures::Error::None )
		{
			return %error;
		}

		%this._animTextureLoop ();

		return $AnimTextures::Error::None;
	}

	function StaticShape::stopAnimTextureLoop ( %this )
	{
		cancel (%this.anim_loop);
	}

	function StaticShape::checkCanAnimate ( %this )
	{
		%error = AnimTextures.validateNumFrames (%this.anim_numFrames);

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

	// Internal use only -- Do NOT call this!
	function StaticShape::_animTextureLoop ( %this )
	{
		cancel (%this.anim_loop);

		%fps = %this.anim_fps;
		%numFrames = %this.anim_numFrames;

		if ( %fps $= "" || %numFrames $= "" || %fps <= 0 || %numFrames <= 0 )
		{
			return;
		}

		%frame = %this.anim_currFrame;

		if ( %frame < 100 )
		{
			%frame = "0" @ %frame;
		}

		if ( %frame < 10 )
		{
			%frame = "0" @ %frame;
		}

		%this.setSkinName (%this.anim_namePrefix @ %frame);
		%this.anim_currFrame++;

		if ( %this.anim_currFrame >= %numFrames )
		{
			%this.anim_currFrame = 0;
		}

		%this.anim_loop = %this.schedule (1000 / %this.anim_fps, "_animTextureLoop");
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

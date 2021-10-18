function AnimTextures::onAdd ( %this )
{
	%this.animTexShapes = new SimSet ();
}

function AnimTextures::onRemove ( %this )
{
	%this.animTexShapes.deleteAll ();
	%this.animTexShapes.delete ();
}

function AnimTextures::createShape ( %this, %data, %namePrefix, %numFrames, %fps )
{
	if ( !%this.animCreateCheck (%numFrames) )
	{
		return 0;
	}

	%shape = new StaticShape ()
	{
		dataBlock = %data;
		anim_currFrame = 0;
		anim_namePrefix = %namePrefix;
	};

	MissionCleanup.add (%shape);
	AnimTextures.animTexShapes.add (%shape);

	%shape.anim_numFrames = %numFrames;
	%shape.anim_fps = (%fps $= "" ? $AnimTextures::DefaultFPS : %fps); 

	%shape.startAnimTextureLoop ();

	return %shape;
}

function AnimTextures::getAnimLoopError ( %this, %numFrames )
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

function AnimTextures::getAnimCreateError ( %this, %numFrames )
{
	if ( %this.animTexShapes.getCount () >= $AnimTextures::MaxShapes )
	{
		return $AnimTextures::Error::ShapeLimit;
	}

	return %this.getAnimLoopError (%numFrames);
}

function AnimTextures::printError ( %this, %error )
{
	switch ( %error )
	{
		case $AnimTextures::Error::ShapeLimit:
			error ("ERROR: Animated texture shape limit reached!");

		case $AnimTextures::Error::MinFrames:
			error ("ERROR: Animated textures must have at least " @ $AnimTextures::MinFrames @ " frame(s)");

		case $AnimTextures::Error::MaxFrames:
			error ("ERROR: Animated textures cannot have more than " @ $AnimTextures::MaxFrames @ " frame(s)");
	}
}

function AnimTextures::animLoopCheck ( %this, %numFrames )
{
	%error = %this.getAnimLoopError (%numFrames);

	if ( %error != $AnimTextures::Error::None )
	{
		%this.printError (%error);
		return false;
	}

	return true;
}

function AnimTextures::animCreateCheck ( %this, %numFrames )
{
	%error = %this.getAnimCreateError (%numFrames);

	if ( %error != $AnimTextures::Error::None )
	{
		%this.printError (%error);
		return false;
	}

	return true;
}

package Support_AnimatedTextures
{
	// We package custom functions so they disappear once the server closes/restarts.
	function AnimTextures_init ()
	{
		$AnimTextures::Version = 1;

		$AnimTextures::MaxShapes = 100;
		$AnimTextures::MinFrames = 2;
		$AnimTextures::MaxFrames = 100;
		$AnimTextures::DefaultFPS = 30;

		$AnimTextures::Error::None = 0;
		$AnimTextures::Error::ShapeLimit = 1;
		$AnimTextures::Error::MinFrames = 2;
		$AnimTextures::Error::MaxFrames = 3;

		if ( !isObject (AnimTextures) )
		{
			MissionCleanup.add (new ScriptObject (AnimTextures));
		}
	}

	function StaticShape::startAnimTextureLoop ( %this )
	{
		if ( !AnimTextures.animLoopCheck (%this.anim_numFrames) )
		{
			return false;
		}

		if ( !AnimTextures.animTexShapes.isMember (%this) )
		{
			error ("ERROR: Shape is not in animated texture set!");
			return false;
		}

		%this._animTextureLoop ();

		return true;
	}

	function StaticShape::stopAnimTextureLoop ( %this )
	{
		cancel (%this.anim_loop);
	}

	// Internal use only -- Do NOT call this.
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

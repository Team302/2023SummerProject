
//====================================================================================================================================================
// Copyright 2023 Lake Orion Robotics FIRST Team 302
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE
// OR OTHER DEALINGS IN THE SOFTWARE.
//====================================================================================================================================================

// Team 302 includes
#include <auton/PrimitiveEnums.h>
#include <auton/PrimitiveFactory.h>
#include <auton/PrimitiveParser.h>
#include <auton/drivePrimitives/VisionDrivePrimitive.h>
#include <auton/drivePrimitives/AutoBalance.h>
#include <auton/drivePrimitives/DriveStop.h>
#include <auton/drivePrimitives/DrivePath.h>
#include <auton/drivePrimitives/DrivePathPlanner.h>
#include <auton/drivePrimitives/DriveHoldPosition.h>
#include <auton/drivePrimitives/IPrimitive.h>
#include <auton/drivePrimitives/ResetPosition.h>
#include <auton/drivePrimitives/ResetPositionPathPlanner.h>

PrimitiveFactory *PrimitiveFactory::m_instance = nullptr;

PrimitiveFactory *PrimitiveFactory::GetInstance()
{
	if (PrimitiveFactory::m_instance == nullptr)
	{														   // If we do not have an instance
		PrimitiveFactory::m_instance = new PrimitiveFactory(); // Create a new instance
	}
	return PrimitiveFactory::m_instance; // Return said instance
}

PrimitiveFactory::PrimitiveFactory() : m_DriveStop(nullptr),
									   m_DriveHoldPosition(nullptr),
									   m_resetPosition(nullptr),
									   m_resetPositionPathPlanner(nullptr),
									   m_autoBalance(nullptr),
									   m_drivePath(nullptr),
									   m_drivePathPlanner(nullptr)
{
}

PrimitiveFactory::~PrimitiveFactory()
{
	PrimitiveFactory::m_instance = nullptr; // todo: do we have to delete this pointer?
}

IPrimitive *PrimitiveFactory::GetIPrimitive(PrimitiveParams *primitivePasser)
{
	IPrimitive *primitive = nullptr;
	switch (primitivePasser->GetID()) // Decides which primitive to get or make
	{
	case DO_NOTHING:
		if (m_DriveStop == nullptr)
		{
			m_DriveStop = new DriveStop();
		}
		primitive = m_DriveStop;
		break;

	case HOLD_POSITION:
		if (m_DriveHoldPosition == nullptr)
		{
			m_DriveHoldPosition = new DriveHoldPosition();
		}
		primitive = m_DriveHoldPosition;
		break;

	case RESET_POSITION:
		if (m_resetPosition == nullptr)
		{
			m_resetPosition = new ResetPosition();
		}
		primitive = m_resetPosition;
		break;

	case RESET_POSITION_PATH_PLANNER:
		if (m_resetPositionPathPlanner == nullptr)
		{
			m_resetPositionPathPlanner = new ResetPositionPathPlanner();
		}
		primitive = m_resetPositionPathPlanner;
		break;

	case DRIVE_PATH:
		if (m_drivePath == nullptr)
		{
			m_drivePath = new DrivePath();
		}
		primitive = m_drivePath;
		break;

	case DRIVE_PATH_PLANNER:
		if (m_drivePath == nullptr)
		{
			m_drivePath = new DrivePathPlanner();
		}
		primitive = m_drivePath;
		break;

	case AUTO_BALANCE:
		if (m_autoBalance == nullptr)
		{
			m_autoBalance = new AutoBalance();
		}
		primitive = m_autoBalance;
		break;

	case VISION_ALIGN:
		if (m_visionAlign == nullptr)
		{
			m_visionAlign = new VisionDrivePrimitive();
		}
		primitive = m_visionAlign;
		break;

	default:
		break;
	}
	return primitive;
}

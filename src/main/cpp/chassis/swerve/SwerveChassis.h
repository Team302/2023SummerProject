
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

#pragma once
#include <map>
#include <memory>
#include <string>

#include <frc/BuiltInAccelerometer.h>
#include <frc/estimator/SwerveDrivePoseEstimator.h>
#include <frc/geometry/Pose2d.h>
#include <frc/geometry/Rotation2d.h>
#include <frc/geometry/Translation2d.h>
#include <frc/kinematics/ChassisSpeeds.h>
#include <frc/kinematics/SwerveDriveKinematics.h>
#include <frc/kinematics/SwerveDriveOdometry.h>
#include <frc/kinematics/SwerveModuleState.h>

#include "units/acceleration.h"
#include "units/angle.h"
#include <units/angular_acceleration.h>
#include "units/angular_velocity.h"
#include "units/length.h"
#include "units/velocity.h"

#include <chassis/ChassisOptionEnums.h>
#include <chassis/DragonTargetFinder.h>
#include <chassis/swerve/driveStates/ISwerveDriveState.h>
#include <chassis/swerve/headingStates/ISwerveDriveOrientation.h>
#include <chassis/IChassis.h>
#include "chassis/PoseEstimatorEnum.h"
#include <chassis/swerve/SwerveModule.h>
#include <chassis/ChassisMovement.h>
#include <DragonVision/DragonVision.h>
#include "hw/interfaces/IDragonPigeon.h"

class RobotDrive;

class SwerveChassis : public IChassis
{
public:
    /// @brief Construct a swerve chassis
    /// @param [in] SwerveModule*           frontleft:          front left swerve module
    /// @param [in] SwerveModule*           frontright:         front right swerve module
    /// @param [in] SwerveModule*           backleft:           back left swerve module
    /// @param [in] SwerveModule*           backright:          back right swerve module
    /// @param [in] units::length::inch_t                   wheelDiameter:      Diameter of the wheel
    /// @param [in] units::length::inch_t                   wheelBase:          distance between the front and rear wheels
    /// @param [in] units::length::inch_t                   track:              distance between the left and right wheels
    /// @param [in] units::velocity::meters_per_second_t    maxSpeed:           maximum linear speed of the chassis
    /// @param [in] units::radians_per_second_t             maxAngularSpeed:    maximum rotation speed of the chassis
    /// @param [in] double                                  maxAcceleration:    maximum acceleration in meters_per_second_squared
    SwerveChassis(
        SwerveModule *frontLeft,
        SwerveModule *frontRight,
        SwerveModule *backLeft,
        SwerveModule *backRight,
        units::length::inch_t wheelBase,
        units::length::inch_t track,
        units::velocity::meters_per_second_t maxSpeed,
        units::radians_per_second_t maxAngularSpeed,
        units::acceleration::meters_per_second_squared_t maxAcceleration,
        units::angular_acceleration::radians_per_second_squared_t maxAngularAcceleration,
        std::string networkTableName);

    ~SwerveChassis() noexcept override = default;

    void InitStates();

    /// @brief Align all of the swerve modules to point forward
    void ZeroAlignSwerveModules();

    /// @brief Drive the chassis
    void Drive(ChassisMovement moveInfo) override;

    void Drive() override;

    /// @brief update the chassis odometry based on current states of the swerve modules and the pigeon
    void UpdateOdometry();

    /// @brief Provide the current chassis speed information
    frc::ChassisSpeeds GetChassisSpeeds() const;

    /// @brief Sets of the motor encoders to zero
    void SetEncodersToZero();

    /// @brief Get encoder values
    double GetEncoderValues(SwerveModule *motor);

    /// @brief Reset the current chassis pose based on the provided pose (the rotation comes from the Pigeon)
    /// @param [in] const Pose2d&       pose        Current XY position
    void ResetPose(const frc::Pose2d &pose) override;

    /// @brief Reset yaw to 0 or 180 degrees depending on alliance
    void ResetYaw();

    // static constexpr auto MaxSpeed = 3.0_mps;
    // static constexpr units::angular_velocity::radians_per_second_t MaxAngularSpeed{wpi::numbers::pi};

    units::length::inch_t GetWheelDiameter() const override;
    units::length::inch_t GetWheelBase() const { return m_wheelBase; }
    units::length::inch_t GetTrack() const { return m_track; }
    units::velocity::meters_per_second_t GetMaxSpeed() const { return m_maxSpeed; }
    units::radians_per_second_t GetMaxAngularSpeed() const { return m_maxAngularSpeed; }
    units::acceleration::meters_per_second_squared_t GetMaxAcceleration() const { return m_maxAcceleration; }
    units::angular_acceleration::radians_per_second_squared_t GetMaxAngularAcceleration() const { return m_maxAngularAcceleration; }
    SwerveModule *GetFrontLeft() const { return m_frontLeft; }
    SwerveModule *GetFrontRight() const { return m_frontRight; }
    SwerveModule *GetBackLeft() const { return m_backLeft; }
    SwerveModule *GetBackRight() const { return m_backRight; }
    frc::Pose2d GetPose() const;
    units::angle::degree_t GetYaw() const override;
    units::angle::degree_t GetPitch() const;
    units::angle::degree_t GetRoll() const;

    frc::SwerveDriveKinematics<4> GetKinematics() const { return m_kinematics; }

    // Dummy functions for IChassis Implementation
    inline IChassis::CHASSIS_TYPE GetType() const override { return IChassis::CHASSIS_TYPE::SWERVE; };
    inline void Initialize() override{};

    void SetTargetHeading(units::angle::degree_t targetYaw) override;

    void SetStoredHeading(units::angle::degree_t heading);
    units::angle::degree_t GetStoredHeading() { return m_storedYaw; };

    ISwerveDriveOrientation *GetSpecifiedHeadingState(ChassisOptionEnums::HeadingOption headingOption);
    ISwerveDriveState *GetSpecifiedDriveState(ChassisOptionEnums::DriveStateType driveOption);

    ISwerveDriveOrientation *GetHeadingState(ChassisMovement moveInfo);

private:
    ISwerveDriveState *GetDriveState(ChassisMovement moveInfo);

    frc::ChassisSpeeds GetFieldRelativeSpeeds(
        units::meters_per_second_t xSpeed,
        units::meters_per_second_t ySpeed,
        units::radians_per_second_t rot);

    units::angular_velocity::degrees_per_second_t CalcHeadingCorrection(
        units::angle::degree_t targetAngle,
        double kP);

    SwerveModule *m_frontLeft;
    SwerveModule *m_frontRight;
    SwerveModule *m_backLeft;
    SwerveModule *m_backRight;

    RobotDrive *m_robotDrive;
    std::map<ChassisOptionEnums::DriveStateType, ISwerveDriveState *> m_driveStateMap;
    std::map<ChassisOptionEnums::HeadingOption, ISwerveDriveOrientation *> m_headingStateMap;

    frc::SwerveModuleState m_flState;
    frc::SwerveModuleState m_frState;
    frc::SwerveModuleState m_blState;
    frc::SwerveModuleState m_brState;

    units::length::inch_t m_wheelBase;
    units::length::inch_t m_track;
    units::velocity::meters_per_second_t m_maxSpeed;
    units::radians_per_second_t m_maxAngularSpeed;
    units::acceleration::meters_per_second_squared_t m_maxAcceleration;
    units::angular_acceleration::radians_per_second_squared_t m_maxAngularAcceleration;

    IDragonPigeon *m_pigeon;
    frc::BuiltInAccelerometer m_accel;
    bool m_runWPI;
    PoseEstimatorEnum m_poseOpt;
    frc::Pose2d m_pose;
    units::angle::degree_t m_offsetPoseAngle;
    units::velocity::meters_per_second_t m_drive;
    units::velocity::meters_per_second_t m_steer;
    units::angular_velocity::radians_per_second_t m_rotate;

    static constexpr units::velocity::meters_per_second_t m_velocityDeadband = units::velocity::meters_per_second_t(0.025);
    static constexpr units::angular_velocity::radians_per_second_t m_angularDeadband = units::angular_velocity::radians_per_second_t(0.1);

    frc::Translation2d m_frontLeftLocation;
    frc::Translation2d m_frontRightLocation;
    frc::Translation2d m_backLeftLocation;
    frc::Translation2d m_backRightLocation;

    frc::SwerveDriveKinematics<4> m_kinematics;

    frc::SwerveDrivePoseEstimator<4> m_poseEstimator;

    units::angle::degree_t m_storedYaw;
    units::angular_velocity::degrees_per_second_t m_yawCorrection;

    DragonTargetFinder m_targetFinder;
    units::angle::degree_t m_targetHeading;
    DragonVision *m_vision;

    ISwerveDriveState *m_currentDriveState;
    ISwerveDriveOrientation *m_currentOrientationState;

    bool m_initialized = false;
    bool m_hasResetToVisionTarget = false;
    std::string m_networkTableName;
};
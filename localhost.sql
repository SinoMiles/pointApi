-- phpMyAdmin SQL Dump
-- version 4.8.5
-- https://www.phpmyadmin.net/
--
-- 主机： localhost
-- 生成日期： 2024-06-14 15:36:49
-- 服务器版本： 5.7.28
-- PHP 版本： 7.3.11

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- 数据库： `g`
--
CREATE DATABASE IF NOT EXISTS `g` DEFAULT CHARACTER SET utf8 COLLATE utf8_unicode_ci;
USE `g`;

-- --------------------------------------------------------

--
-- 表的结构 `grid`
--

CREATE TABLE `grid` (
  `x` int(11) DEFAULT NULL,
  `y` int(11) DEFAULT NULL,
  `is_disabled` tinyint(1) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- 转存表中的数据 `grid`
--

INSERT INTO `grid` (`x`, `y`, `is_disabled`) VALUES
(1, 1, 0),
(2, 1, 0),
(3, 1, 0),
(4, 1, 0),
(5, 1, 0),
(6, 1, 1),
(7, 1, 1),
(8, 1, 1),
(9, 1, 1),
(10, 1, 1),
(1, 2, 0),
(2, 2, 0),
(3, 2, 0),
(4, 2, 0),
(5, 2, 0),
(6, 2, 1),
(7, 2, 1),
(8, 2, 1),
(9, 2, 1),
(10, 2, 1),
(1, 3, 0),
(2, 3, 0),
(3, 3, 0),
(4, 3, 0),
(5, 3, 0),
(6, 3, 1),
(7, 3, 1),
(8, 3, 1),
(9, 3, 1),
(10, 3, 1),
(1, 4, 0),
(2, 4, 0),
(3, 4, 0),
(4, 4, 0),
(5, 4, 0),
(6, 4, 1),
(7, 4, 1),
(8, 4, 1),
(9, 4, 1),
(10, 4, 1),
(1, 5, 0),
(2, 5, 0),
(3, 5, 0),
(4, 5, 0),
(5, 5, 0),
(6, 5, 1),
(7, 5, 1),
(8, 5, 1),
(9, 5, 1),
(10, 5, 1),
(1, 6, 1),
(2, 6, 1),
(3, 6, 1),
(4, 6, 1),
(5, 6, 1),
(6, 6, 1),
(7, 6, 1),
(8, 6, 1),
(9, 6, 1),
(10, 6, 1),
(1, 7, 1),
(2, 7, 1),
(3, 7, 1),
(4, 7, 1),
(5, 7, 1),
(6, 7, 1),
(7, 7, 1),
(8, 7, 1),
(9, 7, 1),
(10, 7, 1),
(1, 8, 1),
(2, 8, 1),
(3, 8, 1),
(4, 8, 1),
(5, 8, 1),
(6, 8, 1),
(7, 8, 1),
(8, 8, 1),
(9, 8, 1),
(10, 8, 1),
(1, 9, 1),
(2, 9, 1),
(3, 9, 1),
(4, 9, 1),
(5, 9, 1),
(6, 9, 1),
(7, 9, 1),
(8, 9, 1),
(9, 9, 1),
(10, 9, 1),
(1, 10, 1),
(2, 10, 1),
(3, 10, 1),
(4, 10, 1),
(5, 10, 1),
(6, 10, 1),
(7, 10, 1),
(8, 10, 1),
(9, 10, 1),
(10, 10, 1);
--
-- 数据库： `point`
--
CREATE DATABASE IF NOT EXISTS `point` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;
USE `point`;

-- --------------------------------------------------------

--
-- 表的结构 `activities`
--

CREATE TABLE `activities` (
  `ActivityId` int(11) NOT NULL,
  `Title` varchar(100) NOT NULL,
  `Description` varchar(500) NOT NULL,
  `StartTime` datetime(6) NOT NULL,
  `EndTime` datetime(6) NOT NULL,
  `Location` varchar(100) NOT NULL,
  `Latitude` double NOT NULL,
  `Longitude` double NOT NULL,
  `MaxParticipants` int(11) NOT NULL,
  `CurrentParticipants` int(11) NOT NULL,
  `TypeId` int(11) NOT NULL,
  `IsPaid` tinyint(1) NOT NULL,
  `FeeAmount` decimal(18,2) DEFAULT NULL,
  `FeeDescription` varchar(100) NOT NULL,
  `Status` int(11) NOT NULL,
  `OrganizerId` int(11) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) NOT NULL,
  `ApprovedParticipantId` int(11) DEFAULT NULL,
  `IsApprovedParticipantVisible` tinyint(1) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- 表的结构 `activitystep`
--

CREATE TABLE `activitystep` (
  `StepId` int(11) NOT NULL,
  `ActivityId` int(11) NOT NULL,
  `StepStartTime` datetime(6) NOT NULL,
  `StepEndTime` datetime(6) NOT NULL,
  `Description` varchar(200) NOT NULL,
  `StepOrder` int(11) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- 表的结构 `activitytypes`
--

CREATE TABLE `activitytypes` (
  `TypeId` int(11) NOT NULL,
  `Name` varchar(50) NOT NULL,
  `Description` varchar(200) DEFAULT NULL,
  `ParentTypeId` int(11) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- 表的结构 `lotteryactivitys`
--

CREATE TABLE `lotteryactivitys` (
  `Id` int(11) NOT NULL,
  `Name` longtext NOT NULL,
  `image` longtext,
  `Description` longtext NOT NULL,
  `StartDate` datetime(6) NOT NULL,
  `EndDate` datetime(6) NOT NULL,
  `MaxParticipants` int(11) NOT NULL,
  `IsOpen` tinyint(1) NOT NULL,
  `HaveParticipants` int(11) NOT NULL,
  `CreateTime` datetime(6) NOT NULL,
  `CreatorUserId` int(11) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- 表的结构 `participants`
--

CREATE TABLE `participants` (
  `Id` int(11) NOT NULL,
  `UserId` int(11) NOT NULL,
  `NickName` longtext,
  `Image` longtext,
  `RegistrationDate` datetime(6) NOT NULL,
  `LotteryActivityId` int(11) NOT NULL,
  `Winner` tinyint(1) NOT NULL,
  `Code` longtext,
  `Verify` tinyint(1) NOT NULL,
  `ActivityId` int(11) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- 表的结构 `Users`
--

CREATE TABLE `Users` (
  `Id` int(11) NOT NULL,
  `UserName` longtext,
  `Password` longtext,
  `OpenId` longtext,
  `Phone` longtext,
  `NickName` longtext,
  `HeadImgUrl` longtext,
  `IsMerchant` tinyint(1) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

--
-- 转存表中的数据 `Users`
--

INSERT INTO `Users` (`Id`, `UserName`, `Password`, `OpenId`, `Phone`, `NickName`, `HeadImgUrl`, `IsMerchant`) VALUES
(1, NULL, NULL, 'oluNE4_8U00I1rGE65Q8ofJ5pnFM', '19916244826', '用户昵称', 'https://img.zcool.cn/community/01460b57e4a6fa0000012e7ed75e83.png', 0);

-- --------------------------------------------------------

--
-- 表的结构 `__EFMigrationsHistory`
--

CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;

--
-- 转存表中的数据 `__EFMigrationsHistory`
--

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`) VALUES
('20240612132700_Add', '5.0.11');

--
-- 转储表的索引
--

--
-- 表的索引 `activities`
--
ALTER TABLE `activities`
  ADD PRIMARY KEY (`ActivityId`),
  ADD KEY `IX_Activities_ApprovedParticipantId` (`ApprovedParticipantId`),
  ADD KEY `IX_Activities_OrganizerId` (`OrganizerId`),
  ADD KEY `IX_Activities_TypeId` (`TypeId`);

--
-- 表的索引 `activitystep`
--
ALTER TABLE `activitystep`
  ADD PRIMARY KEY (`StepId`),
  ADD KEY `IX_ActivityStep_ActivityId` (`ActivityId`);

--
-- 表的索引 `activitytypes`
--
ALTER TABLE `activitytypes`
  ADD PRIMARY KEY (`TypeId`),
  ADD KEY `IX_ActivityTypes_ParentTypeId` (`ParentTypeId`);

--
-- 表的索引 `lotteryactivitys`
--
ALTER TABLE `lotteryactivitys`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_LotteryActivitys_CreatorUserId` (`CreatorUserId`);

--
-- 表的索引 `participants`
--
ALTER TABLE `participants`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Participants_ActivityId` (`ActivityId`);

--
-- 表的索引 `Users`
--
ALTER TABLE `Users`
  ADD PRIMARY KEY (`Id`);

--
-- 表的索引 `__EFMigrationsHistory`
--
ALTER TABLE `__EFMigrationsHistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- 在导出的表使用AUTO_INCREMENT
--

--
-- 使用表AUTO_INCREMENT `activities`
--
ALTER TABLE `activities`
  MODIFY `ActivityId` int(11) NOT NULL AUTO_INCREMENT;

--
-- 使用表AUTO_INCREMENT `activitystep`
--
ALTER TABLE `activitystep`
  MODIFY `StepId` int(11) NOT NULL AUTO_INCREMENT;

--
-- 使用表AUTO_INCREMENT `activitytypes`
--
ALTER TABLE `activitytypes`
  MODIFY `TypeId` int(11) NOT NULL AUTO_INCREMENT;

--
-- 使用表AUTO_INCREMENT `lotteryactivitys`
--
ALTER TABLE `lotteryactivitys`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- 使用表AUTO_INCREMENT `participants`
--
ALTER TABLE `participants`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- 使用表AUTO_INCREMENT `Users`
--
ALTER TABLE `Users`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

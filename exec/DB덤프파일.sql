USE bugtopia;

CREATE TABLE `users` (
    `user_id` BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `device_id` VARCHAR(512) NOT NULL,
    `nickname` VARCHAR(15) NOT NULL,
    `created_date` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_date` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE `area` (
    `area_id` BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `area_name` ENUM('FOREST', 'WATER', 'GARDEN') NOT NULL,
    `is_rain` BOOLEAN NOT NULL DEFAULT false
);

CREATE TABLE `event` (
    `event_id` BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `event_name` ENUM('START', 'FOOD_C1', 'FOOD_C2', 'TERITORY_C1', 'TERITORY_C2',  'MARRY') NOT NULL,
    `event_score` INT NOT NULL
);

CREATE TABLE `insects` (
    `insect_id` BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `insect_kr_name` VARCHAR(50) NOT NULL,
    `insect_eng_name` VARCHAR(100) NOT NULL,
    `insect_info` VARCHAR(512) NOT NULL,
    `can_raise` BOOLEAN NOT NULL,
    `rejected_reason` VARCHAR(200),
    `area_id` BIGINT NULL,
    `food` ENUM('JELLY', 'FRUIT', 'HONEY', 'INSECT') NULL,
    `family` VARCHAR(100) NOT NULL
);

CREATE TABLE `raising_insects` (
    `raising_insect_id` BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `user_id` BIGINT NOT NULL,
    `insect_nickname` VARCHAR(15) NOT NULL,
    `insect_id` BIGINT NOT NULL,
    `feed_cnt` INT NOT NULL DEFAULT 0,
    `interact_cnt` INT NOT NULL DEFAULT 0,
    `state` ENUM('RAISE', 'DONE', 'RELEASE') NOT NULL,
    `created_date` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_date` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    `continuous_days` INT NOT NULL DEFAULT 1,
    `event_id` BIGINT NOT NULL
);

CREATE TABLE `insect_love_score` (
    `insect_love_score_id` BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `collected_insect_id` BIGINT NOT NULL,
    `category` ENUM('FOOD', 'WEATHER', 'INTERACTION') NOT NULL,
    `created_date` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE `notifications` (
    `notification_id` BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `raising_insect_id` BIGINT NOT NULL,
    `type` ENUM('HUNGRY', 'RAINING', 'ATTACK', 'ATTENDANCE', 'BABY', 'GREETING', 'MEMORY', 'THANKS') NOT NULL,
    `is_read` BOOLEAN DEFAULT false,
    `created_date` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE `catched_insects` (
    `catched_insect_id` BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `user_id` BIGINT NOT NULL,
    `insect_id` BIGINT NOT NULL,
    `catched_date` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `photo` VARCHAR(512) NOT NULL,
    `state` ENUM('POSSIBLE', 'IMPOSSIBLE', 'RAISED') NOT NULL
);

CREATE TABLE `eggs` (
    `egg_id` BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `user_id` BIGINT NOT NULL,
    `insect_id` BIGINT NOT NULL,
    `parent_nickname` VARCHAR(15) NOT NULL,
    `state` INT NOT NULL DEFAULT 0,
    `created_date` TIMESTAMP NOT NULL
);
CREATE TABLE user_info ( -- Holds information on each user
	user_info_id CHAR(32) NOT NULL UNIQUE,
	user_name TEXT NOT NULL,
	first_name TEXT NOT NULL,
	last_name TEXT NOT NULL,

	CONSTRAINT pk_user_info PRIMARY KEY (user_info_id)
);

CREATE TABLE course_info ( -- Holds information on each course
	course_info_id CHAR(32) NOT NULL UNIQUE,
	course_name TEXT NOT NULL,

	CONSTRAINT pk_course_info PRIMARY KEY (course_info_id)
);

CREATE TABLE course_users ( -- Holds information on the users that are in a course
	course_info_id CHAR(32) NOT NULL,
	user_info_id CHAR(32) NOT NULL,

	CONSTRAINT pk_course_users PRIMARY KEY (course_info_id, user_info_id)
);

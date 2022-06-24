drop table if exists dbo.subscriptions;
drop table if exists dbo.notifications;
drop table if exists dbo.transactions;
drop table if exists dbo.debts;
drop table if exists dbo.user_categories;
drop table if exists dbo.categories;
drop table if exists dbo.accounts;
drop table if exists dbo.restore_passwords;
drop table if exists dbo.refresh_tokens;
drop table if exists dbo.users;
drop table if exists enum.notification_types;
drop table if exists enum.group_roles;

create table enum.notification_types (
	id int NOT NULL,
	name varchar(50) NOT NULL,
    CONSTRAINT pk_notification_types PRIMARY KEY (id)
);

create table enum.group_roles (
	id int NOT NULL,
	name varchar(50) NOT NULL,
    CONSTRAINT pk_group_roles PRIMARY KEY (id)
);

INSERT INTO enum.group_roles(id, name) VALUES (0, 'Unknown'),(1, 'Admin'),(2, 'Editor'),(3, 'Reader');

create table enum.message_types (
	id int NOT NULL,
	name varchar(50) NOT NULL,
    CONSTRAINT pk_message_types PRIMARY KEY (id)
);

INSERT INTO enum.message_types(id, name) VALUES (0, 'Unknown'),(1, 'Email'),(2, 'SMS');

create table enum.message_statuses (
	id int NOT NULL,
	name varchar(50) NOT NULL,
    CONSTRAINT pk_message_statuses PRIMARY KEY (id)
);

INSERT INTO enum.message_statuses(id, name) VALUES (0, 'Unknown'),(1, 'Created'),(2, 'InProgress'),(3, 'Error'),(4, 'Sent');

create table dbo.messages (
	id int NOT NULL GENERATED ALWAYS AS IDENTITY,
	receiver varchar(250) NOT NULL,
	title varchar(150) NOT NULL,
	content varchar(10485760) NOT NULL,
	message_type_id int NOT NULL,
	message_status_id int NOT NULL,
    CONSTRAINT pk_messages PRIMARY KEY (id),
    CONSTRAINT fk_messages_message_types FOREIGN KEY (message_type_id) REFERENCES enum.message_types (id),
    CONSTRAINT fk_messages_message_statuses FOREIGN KEY (message_status_id) REFERENCES enum.message_statuses (id)
);

create table dbo.users (
	id int NOT NULL GENERATED ALWAYS AS IDENTITY,
	password varchar(150) NOT NULL,
	name varchar(150) NOT NULL,
	email varchar(150) NOT NULL,
	tel varchar(50),
	is_admin boolean NOT NULL,
    CONSTRAINT pk_users PRIMARY KEY (id)
);

create table dbo.categories (
	id int NOT NULL GENERATED ALWAYS AS IDENTITY,
	name varchar(150) NOT NULL,
	user_id int,
    CONSTRAINT pk_categories PRIMARY KEY (id),
    CONSTRAINT fk_user_categories_user FOREIGN KEY (user_id) REFERENCES dbo.users (id)
);

CREATE TABLE dbo.restore_passwords
(
    id integer NOT NULL GENERATED ALWAYS AS IDENTITY,
    user_id integer NOT NULL,
    user_reset_id varchar(255) NOT NULL,
    expired timestamp NOT NULL,
    CONSTRAINT pk_restore_passwords PRIMARY KEY (id),
    CONSTRAINT fk_restore_passwords_user_id FOREIGN KEY (user_id) REFERENCES dbo.users (id)
);

CREATE TABLE dbo.refresh_tokens
(
    id integer NOT NULL GENERATED ALWAYS AS IDENTITY,
    user_id integer NOT NULL,
    access_token_id varchar(255) NOT NULL,
    expired timestamp NOT NULL,
    CONSTRAINT pk_refresh_token PRIMARY KEY (id),
    CONSTRAINT fk_refresh_tokens_user_id FOREIGN KEY (user_id) REFERENCES dbo.users (id)
);

create table dbo.accounts (
	id int NOT NULL GENERATED ALWAYS AS IDENTITY,
	code varchar(125) NOT NULL,
	currency varchar(25) NOT NULL,
	name varchar(150) NOT NULL,
	amount float NOT NULL,
    created timestamp NOT NULL,
	is_deleted boolean NOT NULL DEFAULT False,
	user_id int NOT NULL,
    CONSTRAINT pk_accounts PRIMARY KEY (id),
    CONSTRAINT fk_accounts_user FOREIGN KEY (user_id) REFERENCES dbo.users (id)
);

create table dbo.notifications (
	id int NOT NULL GENERATED ALWAYS AS IDENTITY,
	parameters varchar NOT NULL,
	name varchar(150) NOT NULL,
	is_read boolean NOT NULL,
	notification_type_id int NOT NULL,
	user_id int NOT NULL,
    CONSTRAINT pk_notifications PRIMARY KEY (id),
    CONSTRAINT fk_notifications_user FOREIGN KEY (user_id) REFERENCES dbo.users (id),
    CONSTRAINT fk_notifications_notification_type FOREIGN KEY (notification_type_id) REFERENCES enum.notification_types (id)
);

create table dbo.transactions (
	id int NOT NULL GENERATED ALWAYS AS IDENTITY,
	amount float NOT NULL,
	date timestamp NOT NULL,
	name varchar(150) NOT NULL,
	account_id int NOT NULL,
	category_id int,
    CONSTRAINT pk_transactions PRIMARY KEY (id),
    CONSTRAINT fk_transactions_account FOREIGN KEY (account_id) REFERENCES dbo.accounts (id),
    CONSTRAINT fk_transactions_category FOREIGN KEY (category_id) REFERENCES dbo.categories (id)
);

create table dbo.subscriptions (
	id int NOT NULL GENERATED ALWAYS AS IDENTITY,
	amount float NOT NULL,
	date_from timestamp NOT NULL,
	date_to timestamp NOT NULL,
	name varchar(150) NOT NULL,
	user_id int NOT NULL,
    CONSTRAINT pk_subscriptions PRIMARY KEY (id),
    CONSTRAINT fk_subscriptions_user FOREIGN KEY (user_id) REFERENCES dbo.users (id)
);

create table dbo.debts (
	id int NOT NULL GENERATED ALWAYS AS IDENTITY,
	amount float NOT NULL,
	created timestamp NOT NULL,
	due_to timestamp,
	name varchar(150) NOT NULL,
	note varchar(500) NOT NULL,
	is_closed bool NOT NULL,
	currency varchar(25) NOT NULL,
	user_id int NOT NULL,
    CONSTRAINT pk_debts PRIMARY KEY (id),
    CONSTRAINT fk_debts_user FOREIGN KEY (user_id) REFERENCES dbo.users (id)
);

create table dbo.goals (
	id int NOT NULL GENERATED ALWAYS AS IDENTITY,
	current_amount float NOT NULL,
	full_amount float NOT NULL,
	created timestamp NOT NULL,
	date_to timestamp,
	name varchar(150) NOT NULL,
	description varchar(500) NOT NULL,
	is_active BOOLEAN NOT NULL,
	currency varchar(25) NOT NULL,
	user_id int NOT NULL,
    CONSTRAINT pk_debts PRIMARY KEY (id),
    CONSTRAINT fk_debts_user FOREIGN KEY (user_id) REFERENCES dbo.users (id)
);

create table dbo.groups (
	id int NOT NULL GENERATED ALWAYS AS IDENTITY,
	name varchar(150) NOT NULL,
	account_id int NOT NULL,
    CONSTRAINT pk_groups PRIMARY KEY (id),
    CONSTRAINT fk_groups_account FOREIGN KEY (account_id) REFERENCES dbo.accounts (id)
);

create table dbo.user_group_roles (
	id int NOT NULL GENERATED ALWAYS AS IDENTITY,
	dateFrom timestamp NOT NULL,
	dateTo timestamp NOT NULL,
	group_id int NOT NULL,
	user_id int NOT NULL,
	group_role_id int NOT NULL,
    CONSTRAINT pk_user_group_roles PRIMARY KEY (id),
    CONSTRAINT fk_user_group_roles_user FOREIGN KEY (user_id) REFERENCES dbo.users (id),
    CONSTRAINT fk_user_group_roles_group FOREIGN KEY (group_id) REFERENCES dbo.groups (id),
    CONSTRAINT fk_user_group_roles_group_role FOREIGN KEY (group_role_id) REFERENCES enum.group_roles (id)
);

create table dbo.subscriptions_bills (
	id int NOT NULL GENERATED ALWAYS AS IDENTITY,
	date timestamp NOT NULL,
	subscription_id int NOT NULL,
    CONSTRAINT pk_subscriptions_bills PRIMARY KEY (id),
    CONSTRAINT fk_subscriptions_bills_subscription FOREIGN KEY (subscription_id) REFERENCES dbo.subscriptions (id)
);


CREATE OR REPLACE FUNCTION dbo.get_created_messages(IN messages_count integer) 
RETURNS SETOF dbo.messages
 AS $$
BEGIN
RETURN QUERY 
	UPDATE dbo.messages as m
	SET message_status_id = 2 -- InProgress
	WHERE m.id in (SELECT sm.id FROM dbo.messages sm
		WHERE sm.message_status_id = 1 -- Created
		LIMIT messages_count)
	RETURNING m.*;
END
$$ LANGUAGE plpgsql;
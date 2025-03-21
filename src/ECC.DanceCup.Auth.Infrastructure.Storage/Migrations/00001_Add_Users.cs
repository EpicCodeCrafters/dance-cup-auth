﻿using ECC.DanceCup.Auth.Infrastructure.Storage.Migrations;
using FluentMigrator;

namespace ECC.DanceCup.Api.Infrastructure.Storage.Migrations;

[Migration(00001, TransactionBehavior.None)]
public class Add_Users : SqlMigration
{
    protected override string? UpSqlCommand =>
        """
        create table if not exists "users" (
            "id" bigserial primary key,
            "version" int not null,
            "created_at" timestamp not null,
            "changed_at" timestamp not null,
            "username" text not null,
            "password_hash" bytea not null,
            "password_salt" bytea not null
        );

        create unique index if not exists "idx_users_username" on "users" ("username");
        """;

    protected override string? DownSqlCommand =>
        """
        drop index concurrently if exists "idx_users_username";
        drop table if exists "users";
        """;
}
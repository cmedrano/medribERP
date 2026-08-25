using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PresupuestoMVC.Migrations
{
    /// <inheritdoc />
    public partial class RenameExpensesTableAndColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budget_CompanyId_Company",
                table: "Budget");

            migrationBuilder.DropForeignKey(
                name: "FK_Budget_CreateByUser_Users",
                table: "Budget");

            migrationBuilder.DropForeignKey(
                name: "FK_Budget_DeleteByUser_Users",
                table: "Budget");

            migrationBuilder.DropForeignKey(
                name: "FK_Budget_UpdateByUser_Users",
                table: "Budget");

            migrationBuilder.DropForeignKey(
                name: "FK_Cuentas_CompanyId_Company",
                table: "Cuentas");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_CompanyId_Company",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_Cuentas_CuentaId",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_CreateByUserId_Users",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_DeleteByUserId_Users",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_UpdateByUserId_Users",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_Periodo",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Income_Cuentas_CuentasId",
                table: "income_transfers");

            migrationBuilder.Sql("""
                DO $$
                DECLARE
                    foreign_key_record record;
                BEGIN
                    FOR foreign_key_record IN
                        SELECT conrelid::regclass AS table_name, conname AS constraint_name
                        FROM pg_constraint
                        WHERE confrelid = '"RubroType"'::regclass
                          AND contype = 'f'
                    LOOP
                        EXECUTE format(
                            'ALTER TABLE %s DROP CONSTRAINT %I',
                            foreign_key_record.table_name,
                            foreign_key_record.constraint_name);
                    END LOOP;
                END $$;
                """);

            migrationBuilder.Sql("""
                DO $$
                DECLARE
                    foreign_key_record record;
                BEGIN
                    FOR foreign_key_record IN
                        SELECT conrelid::regclass AS table_name, conname AS constraint_name
                        FROM pg_constraint
                        WHERE confrelid = '"Cuentas"'::regclass
                          AND contype = 'f'
                    LOOP
                        EXECUTE format(
                            'ALTER TABLE %s DROP CONSTRAINT %I',
                            foreign_key_record.table_name,
                            foreign_key_record.constraint_name);
                    END LOOP;
                END $$;
                """);

            migrationBuilder.Sql("""
                DO $$
                DECLARE
                    primary_key_name text;
                BEGIN
                    SELECT conname
                    INTO primary_key_name
                    FROM pg_constraint
                    WHERE conrelid = '"RubroType"'::regclass
                      AND contype = 'p';

                    IF primary_key_name IS NOT NULL THEN
                        EXECUTE format(
                            'ALTER TABLE "RubroType" DROP CONSTRAINT %I',
                            primary_key_name);
                    END IF;
                END $$;
                """);

            migrationBuilder.DropPrimaryKey(
                name: "Gastos_pkey",
                table: "Gastos");

            migrationBuilder.Sql("""
                DO $$
                DECLARE
                    primary_key_name text;
                BEGIN
                    SELECT conname
                    INTO primary_key_name
                    FROM pg_constraint
                    WHERE conrelid = '"Cuentas"'::regclass
                      AND contype = 'p';

                    IF primary_key_name IS NOT NULL THEN
                        EXECUTE format(
                            'ALTER TABLE "Cuentas" DROP CONSTRAINT %I',
                            primary_key_name);
                    END IF;
                END $$;
                """);

            migrationBuilder.Sql("""
                DO $$
                BEGIN
                    IF to_regclass('"Diary"') IS NOT NULL
                    AND to_regclass('diary') IS NULL THEN
                        ALTER TABLE "Diary" RENAME TO diary;
                    END IF;
                END $$;
                """);

            migrationBuilder.RenameTable(
                name: "Budget",
                newName: "budget");

            migrationBuilder.RenameTable(
                name: "RubroType",
                newName: "category_type");

            migrationBuilder.RenameTable(
                name: "Gastos",
                newName: "expenses");

            migrationBuilder.RenameTable(
                name: "Cuentas",
                newName: "account");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "budget",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "valorInicial",
                table: "budget",
                newName: "initial_value");

            migrationBuilder.RenameColumn(
                name: "ValorGastado",
                table: "budget",
                newName: "value_spent");

            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "budget",
                newName: "update_date");

            migrationBuilder.RenameColumn(
                name: "UpdateByUserId",
                table: "budget",
                newName: "update_by_user_id");

            migrationBuilder.RenameColumn(
                name: "RubroTypeId",
                table: "budget",
                newName: "category_type_id");

            migrationBuilder.RenameColumn(
                name: "Mes",
                table: "budget",
                newName: "month");

            migrationBuilder.RenameColumn(
                name: "DeleteDate",
                table: "budget",
                newName: "delete_date");

            migrationBuilder.RenameColumn(
                name: "DeleteByUserId",
                table: "budget",
                newName: "delete_by_user_id");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "budget",
                newName: "create_date");

            migrationBuilder.RenameColumn(
                name: "CreateByUserId",
                table: "budget",
                newName: "create_by_user_id");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "budget",
                newName: "company_id");

            migrationBuilder.RenameColumn(
                name: "Anio",
                table: "budget",
                newName: "year");

            migrationBuilder.Sql("""
                DO $$
                DECLARE
                    index_record record;
                BEGIN
                    FOR index_record IN
                        SELECT *
                        FROM (VALUES
                            ('IX_Budget_UpdateByUserId', 'IX_budget_update_by_user_id'),
                            ('IX_Budget_RubroTypeId', 'IX_budget_category_type_id'),
                            ('IX_Budget_DeleteByUserId', 'IX_budget_delete_by_user_id'),
                            ('IX_Budget_CreateByUserId', 'IX_budget_create_by_user_id'),
                            ('IX_Budget_CompanyId', 'IX_budget_company_id')
                        ) AS indexes(old_name, new_name)
                    LOOP
                        IF EXISTS (
                            SELECT 1
                            FROM pg_class c
                            JOIN pg_namespace n ON n.oid = c.relnamespace
                            WHERE n.nspname = current_schema()
                              AND c.relname = index_record.old_name
                        )
                        AND NOT EXISTS (
                            SELECT 1
                            FROM pg_class c
                            JOIN pg_namespace n ON n.oid = c.relnamespace
                            WHERE n.nspname = current_schema()
                              AND c.relname = index_record.new_name
                        ) THEN
                            EXECUTE format(
                                'ALTER INDEX %I RENAME TO %I',
                                index_record.old_name,
                                index_record.new_name);
                        END IF;
                    END LOOP;
                END $$;
                """);

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "category_type",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "nombreRubro",
                table: "category_type",
                newName: "category_name");

            migrationBuilder.RenameColumn(
                name: "RubroPadreId",
                table: "category_type",
                newName: "category_father_id");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "category_type",
                newName: "company_id");

            migrationBuilder.Sql("""
                DO $$
                DECLARE
                    index_record record;
                BEGIN
                    FOR index_record IN
                        SELECT *
                        FROM (VALUES
                            ('IX_RubroType_RubroPadreId', 'IX_category_type_category_father_id'),
                            ('IX_RubroType_CompanyId', 'IX_category_type_company_id')
                        ) AS indexes(old_name, new_name)
                    LOOP
                        IF EXISTS (
                            SELECT 1
                            FROM pg_class c
                            JOIN pg_namespace n ON n.oid = c.relnamespace
                            WHERE n.nspname = current_schema()
                              AND c.relname = index_record.old_name
                        )
                        AND NOT EXISTS (
                            SELECT 1
                            FROM pg_class c
                            JOIN pg_namespace n ON n.oid = c.relnamespace
                            WHERE n.nspname = current_schema()
                              AND c.relname = index_record.new_name
                        ) THEN
                            EXECUTE format(
                                'ALTER INDEX %I RENAME TO %I',
                                index_record.old_name,
                                index_record.new_name);
                        END IF;
                    END LOOP;
                END $$;
                """);

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "expenses",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "expenses",
                newName: "update_date");

            migrationBuilder.RenameColumn(
                name: "UpdateByUserId",
                table: "expenses",
                newName: "update_by_user_id");

            migrationBuilder.RenameColumn(
                name: "RubroTypeId",
                table: "expenses",
                newName: "category_type_id");

            migrationBuilder.RenameColumn(
                name: "PeriodoId",
                table: "expenses",
                newName: "period_id");

            migrationBuilder.RenameColumn(
                name: "Nota",
                table: "expenses",
                newName: "note");

            migrationBuilder.RenameColumn(
                name: "Monto",
                table: "expenses",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "Fecha",
                table: "expenses",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "DeleteDate",
                table: "expenses",
                newName: "delete_date");

            migrationBuilder.RenameColumn(
                name: "DeleteByUserId",
                table: "expenses",
                newName: "delete_by_user_id");

            migrationBuilder.RenameColumn(
                name: "CuentaId",
                table: "expenses",
                newName: "account_id");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "expenses",
                newName: "create_date");

            migrationBuilder.RenameColumn(
                name: "CreateByUserId",
                table: "expenses",
                newName: "create_by_user_id");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "expenses",
                newName: "company_id");

            migrationBuilder.Sql("""
                DO $$
                DECLARE
                    index_record record;
                BEGIN
                    FOR index_record IN
                        SELECT *
                        FROM (VALUES
                            ('IX_Gastos_UpdateByUserId', 'IX_expenses_update_by_user_id'),
                            ('IX_Gastos_RubroTypeId', 'IX_expenses_category_type_id'),
                            ('IX_Gastos_PeriodoId', 'IX_expenses_period_id'),
                            ('IX_Gastos_DeleteByUserId', 'IX_expenses_delete_by_user_id'),
                            ('IX_Gastos_CuentaId', 'IX_expenses_account_id'),
                            ('IX_Gastos_CreateByUserId', 'IX_expenses_create_by_user_id'),
                            ('IX_Gastos_CompanyId', 'IX_expenses_company_id')
                        ) AS indexes(old_name, new_name)
                    LOOP
                        IF EXISTS (
                            SELECT 1
                            FROM pg_class c
                            JOIN pg_namespace n ON n.oid = c.relnamespace
                            WHERE n.nspname = current_schema()
                              AND c.relname = index_record.old_name
                        )
                        AND NOT EXISTS (
                            SELECT 1
                            FROM pg_class c
                            JOIN pg_namespace n ON n.oid = c.relnamespace
                            WHERE n.nspname = current_schema()
                              AND c.relname = index_record.new_name
                        ) THEN
                            EXECUTE format(
                                'ALTER INDEX %I RENAME TO %I',
                                index_record.old_name,
                                index_record.new_name);
                        END IF;
                    END LOOP;
                END $$;
                """);

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "account",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "nombreCuenta",
                table: "account",
                newName: "account_name");

            migrationBuilder.RenameColumn(
                name: "SaldoInicial",
                table: "account",
                newName: "initial_balance");

            migrationBuilder.RenameColumn(
                name: "SaldoActual",
                table: "account",
                newName: "current_balance");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "account",
                newName: "company_id");

            migrationBuilder.Sql("""
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM pg_class c
                        JOIN pg_namespace n ON n.oid = c.relnamespace
                        WHERE n.nspname = current_schema()
                          AND c.relname = 'IX_Cuentas_CompanyId'
                    )
                    AND NOT EXISTS (
                        SELECT 1
                        FROM pg_class c
                        JOIN pg_namespace n ON n.oid = c.relnamespace
                        WHERE n.nspname = current_schema()
                          AND c.relname = 'IX_account_company_id'
                    ) THEN
                        ALTER INDEX "IX_Cuentas_CompanyId" RENAME TO "IX_account_company_id";
                    END IF;
                END $$;
                """);

            migrationBuilder.Sql("""
                DO $$
                DECLARE
                    primary_key_record record;
                    existing_primary_key_name text;
                BEGIN
                    FOR primary_key_record IN
                        SELECT *
                        FROM (VALUES
                            ('budget', 'PK_budget'),
                            ('category_type', 'PK_category_type'),
                            ('expenses', 'PK_expenses'),
                            ('account', 'PK_account')
                        ) AS primary_keys(table_name, desired_name)
                    LOOP
                        SELECT conname
                        INTO existing_primary_key_name
                        FROM pg_constraint
                        WHERE conrelid = format('%I', primary_key_record.table_name)::regclass
                          AND contype = 'p';

                        IF existing_primary_key_name IS NULL THEN
                            EXECUTE format(
                                'ALTER TABLE %I ADD CONSTRAINT %I PRIMARY KEY (id)',
                                primary_key_record.table_name,
                                primary_key_record.desired_name);
                        ELSIF existing_primary_key_name <> primary_key_record.desired_name THEN
                            EXECUTE format(
                                'ALTER TABLE %I RENAME CONSTRAINT %I TO %I',
                                primary_key_record.table_name,
                                existing_primary_key_name,
                                primary_key_record.desired_name);
                        END IF;
                    END LOOP;
                END $$;
                """);

            migrationBuilder.AddForeignKey(
                name: "FK_account_Company_company_id",
                table: "account",
                column: "company_id",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_budget_Company_company_id",
                table: "budget",
                column: "company_id",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_budget_Users_create_by_user_id",
                table: "budget",
                column: "create_by_user_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_budget_Users_delete_by_user_id",
                table: "budget",
                column: "delete_by_user_id",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_budget_Users_update_by_user_id",
                table: "budget",
                column: "update_by_user_id",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_budget_category_type_category_type_id",
                table: "budget",
                column: "category_type_id",
                principalTable: "category_type",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_category_type_Company_company_id",
                table: "category_type",
                column: "company_id",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_category_type_category_type_category_father_id",
                table: "category_type",
                column: "category_father_id",
                principalTable: "category_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_expenses_Company_company_id",
                table: "expenses",
                column: "company_id",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_expenses_Users_create_by_user_id",
                table: "expenses",
                column: "create_by_user_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_expenses_Users_delete_by_user_id",
                table: "expenses",
                column: "delete_by_user_id",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_expenses_Users_update_by_user_id",
                table: "expenses",
                column: "update_by_user_id",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_expenses_account_account_id",
                table: "expenses",
                column: "account_id",
                principalTable: "account",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_expenses_category_type_category_type_id",
                table: "expenses",
                column: "category_type_id",
                principalTable: "category_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_expenses_periods_period_id",
                table: "expenses",
                column: "period_id",
                principalTable: "periods",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_income_transfers_to_account_id_account",
                table: "income_transfers",
                column: "to_account_id",
                principalTable: "account",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_account_Company_company_id",
                table: "account");

            migrationBuilder.DropForeignKey(
                name: "FK_budget_Company_company_id",
                table: "budget");

            migrationBuilder.DropForeignKey(
                name: "FK_budget_Users_create_by_user_id",
                table: "budget");

            migrationBuilder.DropForeignKey(
                name: "FK_budget_Users_delete_by_user_id",
                table: "budget");

            migrationBuilder.DropForeignKey(
                name: "FK_budget_Users_update_by_user_id",
                table: "budget");

            migrationBuilder.DropForeignKey(
                name: "FK_budget_category_type_category_type_id",
                table: "budget");

            migrationBuilder.DropForeignKey(
                name: "FK_category_type_Company_company_id",
                table: "category_type");

            migrationBuilder.DropForeignKey(
                name: "FK_category_type_category_type_category_father_id",
                table: "category_type");

            migrationBuilder.DropForeignKey(
                name: "FK_expenses_Company_company_id",
                table: "expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_expenses_Users_create_by_user_id",
                table: "expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_expenses_Users_delete_by_user_id",
                table: "expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_expenses_Users_update_by_user_id",
                table: "expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_expenses_account_account_id",
                table: "expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_expenses_category_type_category_type_id",
                table: "expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_expenses_periods_period_id",
                table: "expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_income_transfers_account_from_account_id",
                table: "income_transfers");

            migrationBuilder.DropForeignKey(
                name: "fk_income_transfers_to_account_id_account",
                table: "income_transfers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_budget",
                table: "budget");

            migrationBuilder.DropPrimaryKey(
                name: "PK_expenses",
                table: "expenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_category_type",
                table: "category_type");

            migrationBuilder.DropPrimaryKey(
                name: "PK_account",
                table: "account");

            migrationBuilder.RenameTable(
                name: "budget",
                newName: "Budget");

            migrationBuilder.RenameTable(
                name: "expenses",
                newName: "Gastos");

            migrationBuilder.RenameTable(
                name: "category_type",
                newName: "RubroType");

            migrationBuilder.RenameTable(
                name: "account",
                newName: "Cuentas");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Budget",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "year",
                table: "Budget",
                newName: "Anio");

            migrationBuilder.RenameColumn(
                name: "value_spent",
                table: "Budget",
                newName: "ValorGastado");

            migrationBuilder.RenameColumn(
                name: "update_date",
                table: "Budget",
                newName: "UpdateDate");

            migrationBuilder.RenameColumn(
                name: "update_by_user_id",
                table: "Budget",
                newName: "UpdateByUserId");

            migrationBuilder.RenameColumn(
                name: "month",
                table: "Budget",
                newName: "Mes");

            migrationBuilder.RenameColumn(
                name: "initial_value",
                table: "Budget",
                newName: "valorInicial");

            migrationBuilder.RenameColumn(
                name: "delete_date",
                table: "Budget",
                newName: "DeleteDate");

            migrationBuilder.RenameColumn(
                name: "delete_by_user_id",
                table: "Budget",
                newName: "DeleteByUserId");

            migrationBuilder.RenameColumn(
                name: "create_date",
                table: "Budget",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "create_by_user_id",
                table: "Budget",
                newName: "CreateByUserId");

            migrationBuilder.RenameColumn(
                name: "company_id",
                table: "Budget",
                newName: "CompanyId");

            migrationBuilder.RenameColumn(
                name: "category_type_id",
                table: "Budget",
                newName: "RubroTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_budget_update_by_user_id",
                table: "Budget",
                newName: "IX_Budget_UpdateByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_budget_delete_by_user_id",
                table: "Budget",
                newName: "IX_Budget_DeleteByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_budget_create_by_user_id",
                table: "Budget",
                newName: "IX_Budget_CreateByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_budget_company_id",
                table: "Budget",
                newName: "IX_Budget_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_budget_category_type_id",
                table: "Budget",
                newName: "IX_Budget_RubroTypeId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Gastos",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "update_date",
                table: "Gastos",
                newName: "UpdateDate");

            migrationBuilder.RenameColumn(
                name: "update_by_user_id",
                table: "Gastos",
                newName: "UpdateByUserId");

            migrationBuilder.RenameColumn(
                name: "period_id",
                table: "Gastos",
                newName: "PeriodoId");

            migrationBuilder.RenameColumn(
                name: "note",
                table: "Gastos",
                newName: "Nota");

            migrationBuilder.RenameColumn(
                name: "delete_date",
                table: "Gastos",
                newName: "DeleteDate");

            migrationBuilder.RenameColumn(
                name: "delete_by_user_id",
                table: "Gastos",
                newName: "DeleteByUserId");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Gastos",
                newName: "Fecha");

            migrationBuilder.RenameColumn(
                name: "create_date",
                table: "Gastos",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "create_by_user_id",
                table: "Gastos",
                newName: "CreateByUserId");

            migrationBuilder.RenameColumn(
                name: "company_id",
                table: "Gastos",
                newName: "CompanyId");

            migrationBuilder.RenameColumn(
                name: "category_type_id",
                table: "Gastos",
                newName: "RubroTypeId");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "Gastos",
                newName: "Monto");

            migrationBuilder.RenameColumn(
                name: "account_id",
                table: "Gastos",
                newName: "CuentaId");

            migrationBuilder.RenameIndex(
                name: "IX_expenses_update_by_user_id",
                table: "Gastos",
                newName: "IX_Gastos_UpdateByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_expenses_period_id",
                table: "Gastos",
                newName: "IX_Gastos_PeriodoId");

            migrationBuilder.RenameIndex(
                name: "IX_expenses_delete_by_user_id",
                table: "Gastos",
                newName: "IX_Gastos_DeleteByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_expenses_create_by_user_id",
                table: "Gastos",
                newName: "IX_Gastos_CreateByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_expenses_company_id",
                table: "Gastos",
                newName: "IX_Gastos_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_expenses_category_type_id",
                table: "Gastos",
                newName: "IX_Gastos_RubroTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_expenses_account_id",
                table: "Gastos",
                newName: "IX_Gastos_CuentaId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "RubroType",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "company_id",
                table: "RubroType",
                newName: "CompanyId");

            migrationBuilder.RenameColumn(
                name: "category_name",
                table: "RubroType",
                newName: "nombreRubro");

            migrationBuilder.RenameColumn(
                name: "category_father_id",
                table: "RubroType",
                newName: "RubroPadreId");

            migrationBuilder.RenameIndex(
                name: "IX_category_type_company_id",
                table: "RubroType",
                newName: "IX_RubroType_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_category_type_category_father_id",
                table: "RubroType",
                newName: "IX_RubroType_RubroPadreId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Cuentas",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "initial_balance",
                table: "Cuentas",
                newName: "SaldoInicial");

            migrationBuilder.RenameColumn(
                name: "current_balance",
                table: "Cuentas",
                newName: "SaldoActual");

            migrationBuilder.RenameColumn(
                name: "company_id",
                table: "Cuentas",
                newName: "CompanyId");

            migrationBuilder.RenameColumn(
                name: "account_name",
                table: "Cuentas",
                newName: "nombreCuenta");

            migrationBuilder.RenameIndex(
                name: "IX_account_company_id",
                table: "Cuentas",
                newName: "IX_Cuentas_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Budget",
                table: "Budget",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gastos",
                table: "Gastos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RubroType",
                table: "RubroType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cuentas",
                table: "Cuentas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Budget_Company_CompanyId",
                table: "Budget",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Budget_RubroType_RubroTypeId",
                table: "Budget",
                column: "RubroTypeId",
                principalTable: "RubroType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Budget_Users_CreateByUserId",
                table: "Budget",
                column: "CreateByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Budget_Users_DeleteByUserId",
                table: "Budget",
                column: "DeleteByUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Budget_Users_UpdateByUserId",
                table: "Budget",
                column: "UpdateByUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cuentas_Company_CompanyId",
                table: "Cuentas",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Company_CompanyId",
                table: "Gastos",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Cuentas_CuentaId",
                table: "Gastos",
                column: "CuentaId",
                principalTable: "Cuentas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_RubroType_RubroTypeId",
                table: "Gastos",
                column: "RubroTypeId",
                principalTable: "RubroType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Users_CreateByUserId",
                table: "Gastos",
                column: "CreateByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Users_DeleteByUserId",
                table: "Gastos",
                column: "DeleteByUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Users_UpdateByUserId",
                table: "Gastos",
                column: "UpdateByUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_periods_PeriodoId",
                table: "Gastos",
                column: "PeriodoId",
                principalTable: "periods",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_income_transfers_Cuentas_from_account_id",
                table: "income_transfers",
                column: "from_account_id",
                principalTable: "Cuentas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_income_transfers_Cuentas_to_account_id",
                table: "income_transfers",
                column: "to_account_id",
                principalTable: "Cuentas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RubroType_Company_CompanyId",
                table: "RubroType",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RubroType_RubroType_RubroPadreId",
                table: "RubroType",
                column: "RubroPadreId",
                principalTable: "RubroType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

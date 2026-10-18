namespace OpenPayments.Sandbox.Infrastructure

open Npgsql
open Npgsql.FSharp
open OpenPayments.Sandbox.Core

type BankRepository(dataSource: NpgsqlDataSource) =
    let readBank (read: RowReader) : Bank =
        {
            Name = read.text "name"
            SwiftCode = read.text "swift_code"
            CountryCode = read.text "country_code"
        }

    interface IBankRepository with
        member _.GetAll() =
            task
                {
                    let! banks =
                        dataSource
                        |> Sql.fromDataSource
                        |> Sql.query "select name, swift_code, country_code from bank order by name"
                        |> Sql.executeAsync readBank
                    return List.toArray banks
                }

        member _.Get(swiftCode: string) =
            task
                {
                    let! banks =
                        dataSource
                        |> Sql.fromDataSource
                        |> Sql.query "select name, swift_code, country_code from bank where swift_code = @swiftCode"
                        |> Sql.parameters [ "swiftCode", Sql.text swiftCode ]
                        |> Sql.executeAsync readBank
                    return List.tryHead banks
                }

namespace OpenPayments.Sandbox.Core

open System.Threading.Tasks

type Bank =
    {
        Name: string
        SwiftCode: string
        CountryCode: string
    }

type IBankRepository =
    abstract GetAll: unit -> Task<Bank array>
    abstract Get: swiftCode: string -> Task<Bank option>

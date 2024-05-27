# Microservices Top-Up Workflow with .NET and Temporal

This project demonstrates a sample backend architecture with four microservices using .NET and Temporal for orchestrating a top-up workflow.

## Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Microservices](#microservices)
    - [Card Service](#card-service)
    - [Commission Service](#commission-service)
    - [Limit Service](#limit-service)
    - [Iban Service](#iban-service)
- [Workflow](#workflow)
- [Setup](#setup)
    - [Prerequisites](#prerequisites)
    - [Running the Services](#running-the-services)
    - [Running the Workflow](#running-the-workflow)
- [Testing](#testing)

## Overview

This project demonstrates a top-up workflow where an amount is withdrawn from a card and topped up to an IBAN account. The workflow involves:
1. Checking the card's limit.
2. Calculating and verifying the commission.
3. Withdrawing the total amount from the card.
4. Topping up the IBAN account.
5. Collecting the commission.

## Architecture

The project consists of the following components:
- **Temporal Server**: Manages the workflow orchestration.
- **Microservices**:
    - Card Service
    - Commission Service
    - Limit Service
    - Iban Service
- **Workflow**: Defines the top-up process using Temporal.

## Microservices

### Card Service

Handles card-related operations such as balance checking and withdrawal.

### Commission Service

Calculates and collects commission for transactions.

### Limit Service

Checks the card limit to ensure sufficient funds are available for transactions.

### Iban Service

Handles the top-up operation to the IBAN account.

## Workflow

The workflow orchestrates the following steps:
1. Calculate the total amount including commission.
2. Check if the card limit allows the transaction.
3. Withdraw the total amount from the card.
4. Top-up the IBAN account with the specified amount.
5. Collect the commission.

## Setup

### Prerequisites

- .NET 8.0 SDK
- Docker
- Temporal CLI
- Temporal Server

### Running the Services

1. **Start Temporal Server**:

   ```bash
   temporal server start-dev
   ```
2. **Clone repository** and navigate to the project directory.

3. **Run the following** commands to start each service:

   ```bash
   cd CardService
   dotnet run
   ```

   ```bash
   cd CommissionService
   dotnet run
   ```

   ```bash
   cd LimitService
   dotnet run
   ```

   ```bash
   cd IbanService
   dotnet run
   ```
4. **Start the Temporal Worker**:

   ```bash
   cd TopUpWorker
   dotnet run
   ```

5. **Testing**

You can test the workflow by running the client, which initiates the top-up process. The console will display the workflow progress and completion status.

   ```bash
   cd TopUpClient
   dotnet run
   ```

You should see output indicating the workflow steps and the successful completion of the top-up process.


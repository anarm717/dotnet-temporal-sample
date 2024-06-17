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
    - [Running the Worker](#running-the-worker)
- [Testing](#testing)

## Overview

This project demonstrates a top-up workflow where an amount is withdrawn from a card and topped up to an IBAN account. The workflow involves:
1. Checking the card's limit.
2. Calculating and verifying the commission.
3. Approval step for human driven process.
4. Withdrawing the total amount from the card.
5. Topping up the IBAN account.
6. Collecting the commission.

## Architecture

The project consists of the following components:
- **Temporal Server**: Manages the workflow orchestration.
- **Microservices**:
    - Card Service
    - Commission Service
    - Limit Service
    - Iban Service
- **Workflow**: In CardService created workflow defines the top-up process using Temporal. Each service has it's activity and different task queue to get tasks ordered in workflow.

## Microservices

### Card Service

Handles card-related operations such as balance checking and withdrawal.

### Commission Service

Calculates and collects commission for transactions.

### Approve Request 

The Approval Request is a human-driven process step within the workflow. At this point, the workflow pauses and waits for a human operator to approve the transaction before continuing. This step ensures that the process can incorporate manual oversight and verification as needed. Once the approval is received via a signal, the workflow resumes and completes the subsequent steps.

### Limit Service

Checks the card limit to ensure sufficient funds are available for transactions.

### Iban Service

Handles the top-up operation to the IBAN account.

## Workflow

The workflow orchestrates the following steps:
1. Calculate the total amount including commission.
2. Check if the card limit allows the transaction.
3. Approval Signal waiting for human being approval. 
3. Withdraw the total amount from the card.
4. Top-up the IBAN account with the specified amount.
5. Collect the commission.

## Setup

### Prerequisites

- .NET 8.0 SDK
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
### Running the Worker

   ```bash
   cd TopUpWorker
   dotnet run
   ```

### Testing

You can test the workflow by running the client, which initiates the top-up process. The console will display the workflow progress and completion status.

   ```bash
   cd TopUpClient
   dotnet run
   ```

### Approve Request

You can approve request by setting workflow Id in Program.cs file and run following commands below.

   ```bash
   cd ApproveRequest
   dotnet run
   ```

You should see output indicating the workflow steps and the successful completion of the top-up process.


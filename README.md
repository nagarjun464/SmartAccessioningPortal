# Smart Accessioning Portal

## Overview
This project is an AI-powered requisition intake and accessioning portal designed to improve healthcare lab workflows.

It allows intake staff (CDOs) to upload requisition forms, extract data automatically, verify information, and capture test tube images before accessioning.

## Problem
In accessioning workflows, requisitions often:
- contain missing or incorrect data
- require manual entry
- increase turnaround times
- reduce operational efficiency
- introduce accessioning bottlenecks
- impact patient reporting timelines

## Solution
This system:
- uploads scanned requisition PDFs/images
- uses AI to extract typed and handwritten data
- auto-fills intake forms
- allows staff to verify and correct data
- captures tube images for traceability
- validates required fields before submission

## Features (MVP)
- Create intake case
- Upload requisition PDF
- Enter patient and kit details
- Save case to SQL Server
- View case summary

## Planned Features
- AI OCR extraction (typed + handwritten)
- Confidence scoring for extracted fields
- Verification UI (side-by-side with PDF)
- Camera capture for tube photos
- Accessioning workflow dashboard
- GCP deployment (Cloud Run, Document AI, Cloud Storage)

## Deployment

### Google Cloud Platform (GCP)
- Google Cloud Run (Frontend + API)
- Docker containerization
- Artifact Registry
- Cloud SQL PostgreSQL (planned)
- Cloud Storage (planned)

### CI/CD
- GitHub repository integration
- Planned GitHub Actions deployment workflow

## Screenshots
- Intake Case Form
- PDF Verification Modal
- Tube Photo Capture
- Swagger API
- GCP Cloud Run Deployment

## Tech Stack
- ASP.NET Core Web API
- Blazor
- SQL Server (SSMS 20)
- Entity Framework Core
- Google Cloud Platform (planned)

## Architecture (High Level)
CDO → Upload Req → AI Extraction → Verification → Tube Photo Capture → Case Submission → Accessioning

## Status
🚧 In development (Phase 1: Core intake and database setup)

## Author
Nagarjun (Healthcare operations → .NET developer transition)

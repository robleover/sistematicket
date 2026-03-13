# osTicket — Manual Completo de Documentación

> **Versión de referencia:** osTicket v1.18.x (Community Edition)  
> **Última actualización del manual:** Marzo 2026  
> **Idioma:** Español

---

## Tabla de contenidos

1. [¿Qué es osTicket?](#1-qué-es-osticket)
2. [Arquitectura del sistema](#2-arquitectura-del-sistema)
3. [Requisitos del sistema](#3-requisitos-del-sistema)
4. [Instalación](#4-instalación)
   - 4.1 [Instalación en Linux (Apache)](#41-instalación-en-linux-apache)
   - 4.2 [Instalación en Windows (IIS)](#42-instalación-en-windows-iis)
   - 4.3 [Instalación con Docker](#43-instalación-con-docker)
5. [Configuración inicial](#5-configuración-inicial)
6. [Panel de administración](#6-panel-de-administración)
   - 6.1 [Ajustes del sistema](#61-ajustes-del-sistema)
   - 6.2 [Correo electrónico](#62-correo-electrónico)
   - 6.3 [Formularios y campos personalizados](#63-formularios-y-campos-personalizados)
   - 6.4 [Departamentos](#64-departamentos)
   - 6.5 [Equipos (Teams)](#65-equipos-teams)
   - 6.6 [Agentes](#66-agentes)
   - 6.7 [Roles y permisos](#67-roles-y-permisos)
   - 6.8 [Temas de ayuda (Help Topics)](#68-temas-de-ayuda-help-topics)
   - 6.9 [SLA — Acuerdos de nivel de servicio](#69-sla--acuerdos-de-nivel-de-servicio)
   - 6.10 [Horarios de atención](#610-horarios-de-atención)
   - 6.11 [Plugins](#611-plugins)
7. [Gestión de tickets](#7-gestión-de-tickets)
   - 7.1 [Ciclo de vida de un ticket](#71-ciclo-de-vida-de-un-ticket)
   - 7.2 [Crear ticket (portal web)](#72-crear-ticket-portal-web)
   - 7.3 [Crear ticket por email](#73-crear-ticket-por-email)
   - 7.4 [Crear ticket desde la interfaz de agente](#74-crear-ticket-desde-la-interfaz-de-agente)
   - 7.5 [Estados de un ticket](#75-estados-de-un-ticket)
   - 7.6 [Prioridades](#76-prioridades)
   - 7.7 [Asignación y reasignación](#77-asignación-y-reasignación)
   - 7.8 [Respuestas y notas internas](#78-respuestas-y-notas-internas)
   - 7.9 [Merging (fusión de tickets)](#79-merging-fusión-de-tickets)
   - 7.10 [Transferencia entre departamentos](#710-transferencia-entre-departamentos)
   - 7.11 [Tickets relacionados](#711-tickets-relacionados)
8. [Portal de clientes (Usuarios finales)](#8-portal-de-clientes-usuarios-finales)
9. [Base de conocimiento (Knowledgebase)](#9-base-de-conocimiento-knowledgebase)
10. [Respuestas predefinidas (Canned Responses)](#10-respuestas-predefinidas-canned-responses)
11. [Filtros de tickets (Ticket Filters)](#11-filtros-de-tickets-ticket-filters)
12. [Plantillas de correo](#12-plantillas-de-correo)
13. [Reportes y estadísticas](#13-reportes-y-estadísticas)
14. [API REST](#14-api-rest)
    - 14.1 [Autenticación](#141-autenticación)
    - 14.2 [Endpoints principales](#142-endpoints-principales)
    - 14.3 [Ejemplos de uso](#143-ejemplos-de-uso)
15. [Configuración avanzada](#15-configuración-avanzada)
    - 15.1 [ost-config.php](#151-ost-configphp)
    - 15.2 [Cron jobs](#152-cron-jobs)
    - 15.3 [LDAP / Active Directory](#153-ldap--active-directory)
    - 15.4 [OAuth2 / SSO](#154-oauth2--sso)
16. [Seguridad](#16-seguridad)
17. [Backups y restauración](#17-backups-y-restauración)
18. [Actualización de versión](#18-actualización-de-versión)
19. [Resolución de problemas comunes](#19-resolución-de-problemas-comunes)
20. [Referencia de estructura de base de datos](#20-referencia-de-estructura-de-base-de-datos)

---

## 1. ¿Qué es osTicket?

**osTicket** es un sistema de tickets de soporte de código abierto (open-source) desarrollado en PHP. Permite a empresas y organizaciones centralizar, gestionar y responder solicitudes de soporte de clientes desde múltiples canales (email, portal web, API, teléfono).

### Características principales

| Característica | Descripción |
|---|---|
| Multi-canal | Email, portal web, API |
| Multi-departamento | Organización por áreas de soporte |
| SLA configurable | Tiempos de respuesta por política |
| Base de conocimiento | Artículos de autoservicio para usuarios |
| Roles y permisos | Control granular de acceso de agentes |
| Formularios dinámicos | Campos personalizados por tipo de ticket |
| Respuestas predefinidas | Plantillas reutilizables para agentes |
| Filtros automáticos | Enrutamiento y acciones automáticas |
| Portal cliente | Seguimiento de tickets por usuarios finales |
| API REST | Integración con sistemas externos |
| Plugin system | Extensibilidad mediante plugins oficiales y de terceros |

### Ediciones disponibles

- **Community Edition (CE):** Gratuita, open-source, self-hosted. Toda la funcionalidad base.
- **Cloud Edition:** SaaS administrado por Enhancesoft. Incluye soporte premium.

---

## 2. Arquitectura del sistema

```
┌─────────────────────────────────────────────────────────┐
│                     USUARIOS FINALES                     │
│         (Portal Web / Email / API externa)               │
└─────────────────┬───────────────────────────────────────┘
                  │ HTTP / SMTP / REST
┌─────────────────▼───────────────────────────────────────┐
│                  SERVIDOR WEB                            │
│              Apache / Nginx / IIS                        │
│    ┌────────────────────────────────────────────┐       │
│    │          PHP 8.x (osTicket Core)           │       │
│    │  ┌──────────┐ ┌─────────┐ ┌──────────────┐ │       │
│    │  │  Portal  │ │  Staff  │ │  Admin Panel │ │       │
│    │  │ (/index) │ │(/scp)   │ │  (/scp/admin)│ │       │
│    │  └──────────┘ └─────────┘ └──────────────┘ │       │
│    │  ┌──────────────────────────────────────┐   │       │
│    │  │           API (/api)                 │   │       │
│    │  └──────────────────────────────────────┘   │       │
│    └────────────────────────────────────────────┘       │
└─────────────────┬───────────────────────────────────────┘
                  │ PDO / MySQLi
┌─────────────────▼───────────────────────────────────────┐
│              MySQL / MariaDB                             │
│         (Base de datos principal)                        │
└─────────────────────────────────────────────────────────┘
```

### Directorios principales

```
osticket/
├── bootstrap/          # Arranque de la aplicación
├── include/            # Clases y librerías core
│   ├── class.*.php     # Modelos principales (Ticket, User, Staff, etc.)
│   └── api.*.php       # Controladores de API
├── scp/                # Staff Control Panel (interfaz de agentes)
│   └── admin.php       # Panel de administración
├── api/                # Endpoints REST
├── upload/             # Archivos adjuntos subidos
├── i18n/               # Traducciones
├── plugins/            # Plugins instalados
├── setup/              # Instalador (eliminar tras instalar)
├── ost-config.php      # Configuración principal
└── index.php           # Portal de clientes
```

---

## 3. Requisitos del sistema

### Servidor

| Componente | Mínimo | Recomendado |
|---|---|---|
| **PHP** | 8.0 | 8.2+ |
| **MySQL** | 5.6 | 8.0+ / MariaDB 10.4+ |
| **Servidor web** | Apache 2.4 | Nginx 1.18+ |
| **RAM** | 512 MB | 2 GB+ |
| **Disco** | 500 MB | 5 GB+ (según adjuntos) |

### Extensiones PHP requeridas

```
pdo_mysql    mbstring    xml          intl
json         gd          gettext      imap
openssl      fileinfo    zip          curl
```

### Verificar extensiones instaladas

```bash
php -m | grep -E "pdo_mysql|mbstring|xml|intl|json|gd|gettext|imap|openssl"
```

---

## 4. Instalación

### 4.1 Instalación en Linux (Apache)

#### Preparar el servidor (Ubuntu/Debian)

```bash
# Actualizar paquetes
sudo apt update && sudo apt upgrade -y

# Instalar Apache, MySQL, PHP
sudo apt install apache2 mysql-server php8.2 -y

# Instalar extensiones PHP
sudo apt install php8.2-mysql php8.2-mbstring php8.2-xml php8.2-intl \
  php8.2-gd php8.2-imap php8.2-curl php8.2-zip php8.2-gettext -y

# Habilitar módulos de Apache
sudo a2enmod rewrite
sudo systemctl restart apache2
```

#### Crear base de datos

```sql
CREATE DATABASE osticket CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
CREATE USER 'osticket'@'localhost' IDENTIFIED BY 'password_seguro';
GRANT ALL PRIVILEGES ON osticket.* TO 'osticket'@'localhost';
FLUSH PRIVILEGES;
```

#### Descargar e instalar osTicket

```bash
cd /var/www/html
wget https://github.com/osTicket/osTicket/releases/download/v1.18.1/osTicket-v1.18.1.zip
unzip osTicket-v1.18.1.zip -d osticket
cd osticket

# Copiar configuración
cp include/ost-sampleconfig.php include/ost-config.php
chmod 0666 include/ost-config.php

# Ajustar permisos
chown -R www-data:www-data /var/www/html/osticket
```

#### Configurar VirtualHost de Apache

```apache
# /etc/apache2/sites-available/osticket.conf
<VirtualHost *:80>
    ServerName soporte.miempresa.com
    DocumentRoot /var/www/html/osticket

    <Directory /var/www/html/osticket>
        Options Indexes FollowSymLinks
        AllowOverride All
        Require all granted
    </Directory>

    ErrorLog ${APACHE_LOG_DIR}/osticket_error.log
    CustomLog ${APACHE_LOG_DIR}/osticket_access.log combined
</VirtualHost>
```

```bash
sudo a2ensite osticket.conf
sudo systemctl reload apache2
```

#### Completar instalación web

1. Navegar a `http://soporte.miempresa.com/setup/`
2. Verificar que todos los requisitos estén en verde
3. Completar el formulario de instalación:
   - Nombre del sistema de soporte
   - Email del administrador
   - Credenciales de base de datos
4. Hacer clic en **Install Now**
5. **Eliminar el directorio de setup por seguridad:**

```bash
sudo rm -rf /var/www/html/osticket/setup/
sudo chmod 0644 /var/www/html/osticket/include/ost-config.php
```

---

### 4.2 Instalación en Windows (IIS)

#### Requisitos previos

1. IIS con módulo URL Rewrite y FastCGI
2. PHP 8.2 para Windows (Non-Thread Safe)
3. MySQL 8.0 Community Server

#### Pasos

```powershell
# Instalar IIS
Enable-WindowsOptionalFeature -Online -FeatureName IIS-WebServerRole, IIS-CGI

# Descargar PHP desde https://windows.php.net/download/
# Extraer en C:\PHP

# Configurar php.ini (copiar php.ini-production a php.ini)
# Habilitar extensiones en php.ini:
# extension=pdo_mysql
# extension=mbstring
# extension=gd
# extension=imap
# etc.
```

Configurar `web.config` para rewrite:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<configuration>
  <system.webServer>
    <rewrite>
      <rules>
        <rule name="osTicket" stopProcessing="true">
          <match url="^(.*)$" />
          <conditions>
            <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
            <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
          </conditions>
          <action type="Rewrite" url="/index.php/{R:1}" />
        </rule>
      </rules>
    </rewrite>
  </system.webServer>
</configuration>
```

---

### 4.3 Instalación con Docker

#### docker-compose.yml

```yaml
version: '3.8'

services:
  db:
    image: mysql:8.0
    environment:
      MYSQL_ROOT_PASSWORD: root_password
      MYSQL_DATABASE: osticket
      MYSQL_USER: osticket
      MYSQL_PASSWORD: osticket_password
    volumes:
      - db_data:/var/lib/mysql
    restart: unless-stopped

  osticket:
    image: osticket/osticket:latest
    depends_on:
      - db
    ports:
      - "80:80"
    environment:
      MYSQL_HOST: db
      MYSQL_DATABASE: osticket
      MYSQL_USER: osticket
      MYSQL_PASSWORD: osticket_password
      SMTP_HOST: mail.miempresa.com
      SMTP_PORT: 587
      SMTP_FROM: soporte@miempresa.com
    volumes:
      - uploads:/var/www/html/upload
      - plugins:/var/www/html/plugins
    restart: unless-stopped

volumes:
  db_data:
  uploads:
  plugins:
```

```bash
docker-compose up -d
# Navegar a http://localhost/setup para completar instalación
```

---

## 5. Configuración inicial

Tras instalar, acceder al **Admin Panel** en `/scp/admin.php` con las credenciales creadas durante el setup.

### Checklist de configuración inicial

- [ ] Configurar nombre y URL del sistema (`Admin > Settings > System`)
- [ ] Configurar email de respuesta principal
- [ ] Crear departamentos base (Soporte Técnico, Facturación, etc.)
- [ ] Crear roles (Admin, Agente Senior, Agente)
- [ ] Crear agentes y asignar departamentos
- [ ] Definir SLAs
- [ ] Configurar horarios de atención
- [ ] Crear temas de ayuda (Help Topics)
- [ ] Configurar email entrante (IMAP/POP3)
- [ ] Configurar email saliente (SMTP)
- [ ] Configurar cron job para procesamiento automático

---

## 6. Panel de administración

Acceso: `https://tu-dominio.com/scp/admin.php`

### 6.1 Ajustes del sistema

**Ruta:** `Admin Panel > Settings > System`

| Campo | Descripción |
|---|---|
| Helpdesk Name | Nombre que aparece en emails y portal |
| Helpdesk URL | URL base del sistema |
| Default Department | Departamento que recibe tickets sin clasificar |
| Default Schedule | Horario laboral por defecto |
| Default Ticket Status | Estado inicial de nuevos tickets (Open) |
| Ticket Lock | Tiempo de bloqueo al abrir un ticket |
| Maximum Open Tickets | Límite de tickets abiertos por usuario |
| Require registration | Obligar registro para crear tickets |
| Enable Rich Text | Habilitar editor HTML en respuestas |
| Storage Backend | Dónde guardar archivos adjuntos (filesystem/S3) |

### 6.2 Correo electrónico

**Ruta:** `Admin Panel > Emails`

#### Configurar email de sistema (saliente)

```
Admin Panel > Emails > Settings

SMTP Host:     mail.miempresa.com
SMTP Port:     587 (TLS) / 465 (SSL) / 25 (sin cifrado)
Authentication: Username + Password
Username:      soporte@miempresa.com
Password:      ****
```

#### Configurar email entrante (IMAP/POP3)

```
Admin Panel > Emails > Add New Email

Name:          Soporte General
Address:       soporte@miempresa.com
Hosting:       IMAP
Host:          imap.miempresa.com
Port:          993
Protocol:      SSL
Username:      soporte@miempresa.com
Password:      ****
Folder:        INBOX
Archive Folder: Processed (para mover emails procesados)
```

#### Campos importantes de email

| Campo | Descripción |
|---|---|
| Auto Responder | Enviar acuse de recibo al usuario |
| Default Department | Departamento al que van tickets de este email |
| Default Priority | Prioridad por defecto |
| Default SLA | SLA aplicado automáticamente |
| Help Topic | Tema de ayuda asignado |

### 6.3 Formularios y campos personalizados

**Ruta:** `Admin Panel > Manage > Forms`

osTicket tiene formularios dinámicos por defecto:
- **Ticket Details:** Datos del ticket (asunto, mensaje, adjuntos)
- **Contact Information:** Datos del usuario (nombre, email, teléfono)

#### Tipos de campo disponibles

| Tipo | Descripción |
|---|---|
| Short Answer | Texto de una línea |
| Long Answer | Área de texto multilínea |
| Date / Date-Time | Selector de fecha |
| Phone | Campo teléfono con extensión |
| Checkbox | Casilla de verificación |
| Choices | Lista desplegable simple |
| Multiple Selection | Selección múltiple |
| File Upload | Carga de archivos |
| Section Break | Separador visual |

#### Agregar campo personalizado

1. `Admin Panel > Manage > Forms > Ticket Details > Add Field`
2. Completar: Label, Variable (nombre interno), Type
3. Marcar **Required** si es obligatorio
4. Configurar **Visibility**: Always / On Create / On Close / Staff Only

### 6.4 Departamentos

**Ruta:** `Admin Panel > Agents > Departments`

Los departamentos son las unidades organizativas que reciben y gestionan tickets.

```
Admin Panel > Agents > Departments > Add New Department

Name:           Soporte Técnico
Type:           Public (visible en portal) / Private (solo staff)
SLA:            Standard SLA
Schedule:       Horario laboral estándar
Manager:        agente@empresa.com
Auto Response:  Activado
Outgoing Email: soporte@miempresa.com
```

### 6.5 Equipos (Teams)

**Ruta:** `Admin Panel > Agents > Teams`

Los equipos permiten agrupar agentes de distintos departamentos.

```
Admin Panel > Agents > Teams > Add New Team

Name:     Equipo VIP
Lead:     agente_senior@empresa.com
Members:  [seleccionar agentes]
```

Los equipos se asignan mediante filtros o manualmente en cada ticket.

### 6.6 Agentes

**Ruta:** `Admin Panel > Agents > Agents`

```
Admin Panel > Agents > Add New Agent

Name:            Juan Pérez
Email:           juan@empresa.com
Username:        juan.perez
Password:        (generar o definir)
Phone:           +56 9 1234 5678

Departamento primario:  Soporte Técnico
Rol en depto primario:  Agente Senior
Departamentos adicionales: Facturación (Agente)
```

#### Permisos especiales de agentes

| Permiso | Descripción |
|---|---|
| Can Delete Tickets | Eliminar tickets permanentemente |
| Can Assign Tickets | Asignar a otros agentes |
| Can Close Tickets | Cerrar tickets |
| Can Edit Tickets | Editar datos del ticket |
| Can Post Reply | Responder tickets |
| Can Transfer Tickets | Mover entre departamentos |
| Can Manage KB | Gestionar base de conocimiento |
| Can View Stats | Ver estadísticas globales |

### 6.7 Roles y permisos

**Ruta:** `Admin Panel > Agents > Roles`

Los roles definen qué acciones puede realizar un agente dentro de un departamento.

Roles por defecto:
- **All Access:** Acceso total
- **Expanded Access:** Sin eliminar tickets
- **Limited Access:** Solo responder y comentar
- **View Only:** Solo lectura
- **Contributor:** Puede gestionar KB

#### Crear rol personalizado

```
Admin Panel > Agents > Roles > Add New Role

Name: Coordinador de Soporte

Tickets:
  [✓] Assign     [✓] Close     [✓] Create    [✓] Delete
  [✓] Edit       [✓] Link      [✓] Mark as Answered
  [✓] Merge      [✓] Post Reply  [✓] Refer   [✓] Release
  [✓] Transfer

Tasks:
  [✓] Assign     [✓] Close     [✓] Create    [✓] Delete
  [✓] Edit       [✓] Post Reply

Knowledgebase:
  [✓] Manage Premade Responses
```

### 6.8 Temas de ayuda (Help Topics)

**Ruta:** `Admin Panel > Manage > Help Topics`

Los Help Topics son las categorías que el usuario final ve al abrir un ticket.

```
Admin Panel > Manage > Help Topics > Add New Help Topic

Topic:           Falla técnica en software
Type:            Public
Status:          Active
Department:      Soporte Técnico
Priority:        Normal
SLA Plan:        Standard
Auto Assignment: (equipo o agente específico)
Form:            (formulario personalizado para este tipo)
```

#### Jerarquía de Help Topics

Se pueden crear sub-temas:
```
Soporte Técnico
├── Software
│   ├── Reportes
│   └── Acceso / Login
├── Hardware
└── Conectividad / Red

Facturación
├── Consulta de factura
└── Corrección de datos
```

### 6.9 SLA — Acuerdos de nivel de servicio

**Ruta:** `Admin Panel > Manage > SLA Plans`

```
Admin Panel > Manage > SLA Plans > Add New SLA Plan

Name:           SLA Crítico
Grace Period:   1 hour     (tiempo antes de que el ticket venza)
Schedule:       24/7       (o usar horario laboral)
Transient:      No         (si sí, no afecta reportes fuera de horario)
```

#### Ejemplos de SLA típicos

| Plan | Respuesta | Horario |
|---|---|---|
| Crítico | 1 hora | 24/7 |
| Alto | 4 horas | Laboral (L-V 9-18) |
| Normal | 8 horas | Laboral |
| Bajo | 3 días | Laboral |

### 6.10 Horarios de atención

**Ruta:** `Admin Panel > Manage > Schedules`

```
Admin Panel > Manage > Schedules > Add New Schedule

Name:     Horario Laboral Chile
Timezone: America/Santiago

Lunes:    09:00 - 18:00
Martes:   09:00 - 18:00
Miércoles: 09:00 - 18:00
Jueves:   09:00 - 18:00
Viernes:  09:00 - 17:00
Sábado:   Cerrado
Domingo:  Cerrado

Feriados: (agregar fechas específicas)
```

### 6.11 Plugins

**Ruta:** `Admin Panel > Manage > Plugins`

Plugins oficiales disponibles:

| Plugin | Descripción |
|---|---|
| `auth-cas` | Autenticación CAS (Central Authentication Service) |
| `auth-ldap` | Autenticación LDAP / Active Directory |
| `auth-oauth2` | Autenticación OAuth2 (Google, Azure AD, etc.) |
| `audit` | Log de auditoría de acciones en el sistema |
| `storage-s3` | Almacenar adjuntos en Amazon S3 |
| `storage-fs` | Almacenamiento en sistema de archivos local |
| `phar-storage` | Gestión de archivos PHAR |

#### Instalar plugin

```bash
# Descargar plugin oficial
cd /var/www/html/osticket/plugins
wget https://github.com/osTicket/osTicket-plugins/releases/download/plugin-name.phar

# En el Admin Panel:
Admin Panel > Manage > Plugins > Add New Plugin
# Seleccionar el archivo .phar o el plugin listado
# Activar el plugin
```

---

## 7. Gestión de tickets

### 7.1 Ciclo de vida de un ticket

```
                    ┌──────────────┐
                    │   CREACIÓN   │
                    │  (Open/New)  │
                    └──────┬───────┘
                           │
              ┌────────────▼────────────┐
              │       ASIGNACIÓN        │
              │  (a agente/equipo/depto)│
              └────────────┬────────────┘
                           │
         ┌─────────────────▼──────────────────┐
         │           EN PROCESO                │
         │  Agente trabaja + responde          │
         │  Usuario puede responder            │
         └──┬──────────────┬──────────────────┘
            │              │
     ┌──────▼──┐      ┌────▼──────────┐
     │RESUELTO │      │   ESPERANDO   │
     │(Resolved│      │ (Awaiting     │
     │ by agent│      │  user reply)  │
     └──────┬──┘      └────┬──────────┘
            │              │ usuario responde
            │         ┌────▼──────────┐
            │         │ REABIERTO     │
            │         │ (Reopened)    │
            │         └───────────────┘
            │
     ┌──────▼──────┐
     │   CERRADO   │
     │  (Closed)   │
     └─────────────┘
```

### 7.2 Crear ticket (portal web)

1. Usuario accede a `https://soporte.miempresa.com`
2. Clic en **Open a New Ticket**
3. Completar formulario:
   - Email address (crea o recupera cuenta de usuario)
   - Full Name
   - Help Topic (categoría)
   - Issue Summary (asunto)
   - Issue Details (descripción)
   - Campos personalizados según el Help Topic
   - Archivos adjuntos
4. Clic en **Create Ticket**
5. Usuario recibe confirmación por email con número de ticket

### 7.3 Crear ticket por email

Cuando un usuario envía un email a la dirección configurada (`soporte@empresa.com`):

1. El cron job o la llamada scheduled ejecuta `api/cron.php`
2. osTicket descarga el email via IMAP/POP3
3. Se aplican **Ticket Filters** (si existen coincidencias)
4. Se crea el ticket con el asunto del email y el cuerpo como mensaje
5. Los adjuntos del email se importan como adjuntos del ticket
6. Si el email es de un usuario existente, se asocia a su cuenta
7. Si es nuevo, se crea la cuenta automáticamente (si está habilitado)

### 7.4 Crear ticket desde la interfaz de agente

```
Staff Panel > New Ticket

User:        buscar por email o crear nuevo
Help Topic:  [seleccionar]
Issue:       [asunto]
Dept:        [departamento]
SLA:         [plan SLA]
Priority:    [baja/normal/alta/urgente]
Source:      Phone / Other
Assign To:   [agente o equipo]

(Completar campos del formulario)

Response:    Mensaje inicial al usuario
Attachments: Archivos adjuntos

[Open] o [Open + Reply] o [Open without Response]
```

### 7.5 Estados de un ticket

| Estado | Descripción | ¿Visible para usuario? |
|---|---|---|
| **Open** | Ticket activo, esperando atención | Sí |
| **Closed** | Ticket resuelto y cerrado | Sí |
| **Resolved** | Marcado como resuelto por agente | Sí |
| **Awaiting Reply** | Esperando respuesta del usuario | Sí |
| **Deleted** | En papelera (no visible para usuario) | No |

Los estados **Awaiting Reply** se asignan automáticamente si el agente usa la opción correspondiente al responder.

### 7.6 Prioridades

| Nivel | SLA Típico | Color en UI |
|---|---|---|
| Low (Baja) | 3 días | Gris |
| Normal | 1 día | Azul |
| High (Alta) | 4 horas | Naranja |
| Emergency (Urgente) | 1 hora | Rojo |

Las prioridades pueden asignarse manualmente o automáticamente mediante filtros.

### 7.7 Asignación y reasignación

```
Ticket > Assign > Agent / Team

Al asignar a un agente:
- El agente recibe email de notificación
- El ticket aparece en su cola personal
- El SLA sigue corriendo

Al asignar a un equipo:
- Todos los miembros del equipo son notificados
- Cualquier miembro del equipo puede tomar el ticket
```

### 7.8 Respuestas y notas internas

#### Respuesta al cliente (Reply)

```
Ticket > Post Reply

Respuesta:    [editor de texto]
Status:       Open / Resolved / Awaiting Reply
Attachments:  Archivos a enviar
Signature:    Firma del agente (configurable por agente)
Alert User:   [✓] Enviar email al usuario
```

#### Nota interna (Internal Note)

Las notas internas solo son visibles para el staff, **nunca para el usuario final**.

```
Ticket > Post Note

Nota:         [texto interno]
Assign To:    (opcionalmente reasignar al agregar nota)
```

Se distinguen visualmente con fondo amarillo en la vista del ticket.

### 7.9 Merging (fusión de tickets)

Permite combinar múltiples tickets del mismo usuario sobre el mismo tema:

```
Ticket A (Principal) > More > Merge

Buscar Ticket B por número o email
Seleccionar:
  - Merge mode: duplicados / hijos / links
  - ¿Mantener ambos tickets activos? 
  - Ticket principal (que queda activo)
```

Tras el merge, la conversación queda unificada y el ticket secundario se cierra o se marca como duplicado.

### 7.10 Transferencia entre departamentos

```
Ticket > Transfer

New Department: [seleccionar]
Reason:         [motivo opcional]
Alert Assignee: [✓]
Alert Department Manager: [✓]
```

### 7.11 Tickets relacionados

```
Ticket > Link Ticket

Ticket #:   [número de ticket a vincular]
Type:       Reference / Duplicate
```

Los tickets vinculados aparecen en la barra lateral del ticket.

---

## 8. Portal de clientes (Usuarios finales)

### Funciones disponibles para usuarios

| Función | Descripción |
|---|---|
| Abrir nuevo ticket | Crear solicitud de soporte |
| Ver mis tickets | Historial de todas las solicitudes |
| Responder ticket | Agregar información a un ticket abierto |
| Ver base de conocimiento | Consultar artículos de autoayuda |
| Búsqueda | Buscar en tickets y KB |

### Configuración del portal

```
Admin Panel > Settings > Users

Allow Registration:      Yes/No
Require Login to Create: Yes/No (controla si deben registrarse)
Email validation:        Yes/No (confirmación de email al registrarse)
Auto Account Creation:   Yes/No (crea cuenta al enviar ticket)
Default User Role:       Registered / Guest
```

### Personalización visual del portal

```
Admin Panel > Settings > Branding

Logo:            Subir imagen del logo
Banner Image:    Imagen de cabecera del portal
Custom CSS:      Estilos CSS adicionales
Welcome Message: Texto de bienvenida en la portada
```

---

## 9. Base de conocimiento (Knowledgebase)

**Ruta Staff:** `Staff Panel > Knowledgebase`  
**Ruta Admin:** `Admin Panel > Knowledgebase`

### Estructura

```
Knowledgebase
├── Categoría: Primeros pasos
│   ├── Artículo: Cómo crear un ticket
│   └── Artículo: Cómo seguir el estado de mi solicitud
├── Categoría: Software
│   ├── Artículo: Cómo acceder al sistema
│   └── FAQ: Preguntas frecuentes de acceso
└── Categoría: Facturación
    └── Artículo: Cómo obtener una factura
```

### Crear artículo

```
Knowledgebase > Add New Article

Category:       Primeros pasos
Title:          Cómo crear un ticket de soporte
Content:        [editor HTML]
Keywords:       ticket, soporte, ayuda, crear
Practice:       Public (visible en portal) / Internal (solo staff)
Status:         Published / Draft

[Save Changes]
```

### Crear FAQ (preguntas frecuentes)

```
Knowledgebase > FAQs > Add New FAQ

Question:  ¿Cuánto tiempo tarda en resolverse mi solicitud?
Answer:    El tiempo de respuesta depende de la prioridad...
Category:  Soporte General
Status:    Published
```

---

## 10. Respuestas predefinidas (Canned Responses)

**Ruta:** `Staff Panel > Knowledgebase > Premade`

Las respuestas predefinidas son plantillas de texto reutilizables para acelerar las respuestas de los agentes.

### Crear respuesta predefinida

```
Knowledgebase > Premade > Add New Response

Title:    Acuse de recibo inicial
Dept:     Soporte Técnico (o "All Departments")
Content:  
  Estimado/a %{ticket.name},

  Hemos recibido su solicitud con número #%{ticket.number}.
  
  Un agente se pondrá en contacto con usted a la brevedad.
  
  Saludos,
  Equipo de Soporte
```

### Variables disponibles en respuestas predefinidas

| Variable | Descripción |
|---|---|
| `%{ticket.number}` | Número del ticket |
| `%{ticket.subject}` | Asunto del ticket |
| `%{ticket.name}` | Nombre del usuario |
| `%{ticket.email}` | Email del usuario |
| `%{ticket.dept.name}` | Nombre del departamento |
| `%{ticket.assigned_to}` | Agente asignado |
| `%{agent.fullname}` | Nombre completo del agente que responde |
| `%{agent.signature}` | Firma del agente |
| `%{url.staff}` | URL del panel de staff |

### Usar respuesta predefinida

En la vista de un ticket:  
`Post Reply > Insert Canned Response > [seleccionar]`

El contenido se inserta en el área de respuesta y puede editarse antes de enviar.

---

## 11. Filtros de tickets (Ticket Filters)

**Ruta:** `Admin Panel > Manage > Ticket Filters`

Los filtros automatizan acciones cuando se crea o actualiza un ticket que cumple condiciones.

### Estructura de un filtro

```
Admin Panel > Manage > Ticket Filters > Add New Filter

Name:       Urgentes VIP
Order:      10 (menor número = mayor prioridad)
Status:     Active
Target:     New Tickets (o Bounce / Messages)
Stop:       [ ] Yes (evitar que otros filtros se apliquen)

CONDICIONES (requiere cumplir TODAS / CUALQUIERA):
  Sender Email     | contains  | @clientevip.com
  Subject          | contains  | URGENTE
  
ACCIONES:
  Assign Priority: Emergency
  Assign Dept:     Soporte Técnico
  Assign Team:     Equipo VIP
  Assign SLA:      SLA Crítico
  Auto Reply:      No
  Tag:             vip, urgente
```

### Condiciones disponibles

| Campo | Operadores disponibles |
|---|---|
| Sender Email | is / is not / contains / starts with / ends with |
| Sender Name | is / contains |
| Subject | is / contains / starts with / ends with |
| Message Body | contains |
| Body Size | is / greater than / less than |
| Header | is / contains |
| Assigned Agent | is / is not |
| Assigned Department | is / is not |

### Acciones disponibles

| Acción | Descripción |
|---|---|
| Assign Priority | Cambiar prioridad |
| Assign Department | Asignar a departamento |
| Assign Team | Asignar a equipo |
| Assign Agent | Asignar a agente específico |
| Assign SLA | Cambiar plan SLA |
| Assign Help Topic | Cambiar tema de ayuda |
| Set Status | Abrir / Resolver / Cerrar |
| Disable Auto-Response | No enviar acuse de recibo |
| Reject Ticket | Rechazar y no crear ticket |
| Send Email | Enviar email personalizado |

---

## 12. Plantillas de correo

**Ruta:** `Admin Panel > Emails > Templates`

Las plantillas controlan todos los emails que envía el sistema.

### Plantillas disponibles

| Plantilla | Disparador |
|---|---|
| New Ticket | Cuando se abre un ticket |
| New Ticket (by staff) | Cuando el staff crea el ticket |
| New Message (to user) | Nuevo mensaje del agente al usuario |
| New Message (to staff) | Nueva actividad en ticket asignado |
| New Note Alert | Nueva nota interna |
| Ticket Assignment | Ticket asignado a agente |
| Ticket Transfer | Ticket transferido a otro depto |
| Overdue Ticket Alert | Ticket próximo a vencer SLA |
| Ticket Close | Ticket cerrado |
| Ticket Reopen | Ticket reabierto |
| Password Reset | Reseteo de contraseña |
| Account Activation | Activación de cuenta nueva |

### Variables en plantillas de email

```
%{ticket.number}        %{ticket.subject}       %{ticket.create_date}
%{ticket.close_date}    %{ticket.status}         %{ticket.priority}
%{ticket.dept.name}     %{ticket.assigned_to}    %{ticket.sla.name}
%{recipient.name}       %{recipient.email}       %{base_url}
%{ticket.thread}        %{url.view}              %{url.reply}
```

### Editar plantilla

```
Admin Panel > Emails > Templates > [seleccionar set] > [plantilla]

Subject: [RE: Ticket #%{ticket.number}] - %{ticket.subject}
Body:    [editor HTML con variables]
```

---

## 13. Reportes y estadísticas

**Ruta:** `Staff Panel > Dashboard` y `Reports`

### Dashboard

El dashboard muestra métricas en tiempo real:

- Tickets abiertos / cerrados hoy
- Tickets vencidos (SLA overdue)
- Tickets por departamento
- Tickets asignados a mí
- Tickets sin asignar

### Reportes disponibles

```
Staff Panel > Reports

1. Overview Report
   - Tickets creados / cerrados por período
   - Agrupación por: día / semana / mes
   - Filtros: fecha, departamento, agente

2. Department Report  
   - Volumen por departamento
   - Tiempo promedio de resolución por depto

3. Agent Report
   - Tickets respondidos por agente
   - Ticket de cierre por agente

4. Help Topic Report
   - Distribución por tema de ayuda
   - Identificar temas más frecuentes

5. SLA Report
   - Cumplimiento de SLA por plan
   - % de tickets dentro / fuera de SLA
```

### Exportar reportes

Los reportes pueden exportarse a **CSV** desde el botón Export en cada reporte.

Para reportes avanzados se recomienda conectar directamente a la base de datos MySQL con herramientas como:
- MySQL Workbench
- Metabase
- Grafana (con plugin MySQL)
- Power BI

---

## 14. API REST

### 14.1 Autenticación

osTicket usa **API Keys** para autenticar peticiones. 

```
Admin Panel > Manage > API Keys > Add New API Key

Name:       Integración CRM
IP Address: 192.168.1.100 (o * para cualquier IP)
Can Create: [✓] Tickets [✓] Users
Key:        [generada automáticamente, ej: A1B2C3D4E5F6...]
```

La clave se envía en el header `X-API-Key` de cada petición.

### 14.2 Endpoints principales

#### Tickets

| Método | Endpoint | Descripción |
|---|---|---|
| `POST` | `/api/tickets.json` | Crear nuevo ticket |
| `GET` | `/api/tickets.json` | Listar tickets (no oficial) |
| `GET` | `/api/tickets/{id}.json` | Ver ticket específico |
| `PUT` | `/api/tickets/{id}.json` | Actualizar ticket |

#### Usuarios

| Método | Endpoint | Descripción |
|---|---|---|
| `POST` | `/api/users.json` | Crear usuario |
| `GET` | `/api/users/{id}.json` | Ver usuario |
| `PUT` | `/api/users/{id}.json` | Actualizar usuario |

> **Nota:** La API oficial de osTicket Community Edition es limitada. Solo soporta creación de tickets y usuarios de forma nativa. Para operaciones avanzadas, usar el [osTicket API (unofficial)](https://github.com/clonemeagain/osticket-api) o acceso directo a BD.

### 14.3 Ejemplos de uso

#### Crear ticket via API

```bash
curl -X POST https://soporte.miempresa.com/api/tickets.json \
  -H "X-API-Key: A1B2C3D4E5F6..." \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Juan Pérez",
    "email": "juan@cliente.com",
    "subject": "Error al iniciar sesión",
    "message": "No puedo acceder al sistema desde esta mañana.",
    "topicId": 3,
    "priorityId": 2,
    "source": "API",
    "attachments": []
  }'
```

Respuesta exitosa:
```json
{
  "id": "XXXXX",
  "number": "123456",
  "message": "Created"
}
```

#### Crear usuario via API

```bash
curl -X POST https://soporte.miempresa.com/api/users.json \
  -H "X-API-Key: A1B2C3D4E5F6..." \
  -H "Content-Type: application/json" \
  -d '{
    "name": "María González",
    "email": "maria@cliente.com",
    "phone": "+56912345678",
    "timezone": "America/Santiago"
  }'
```

#### Python — crear ticket

```python
import requests, json

API_KEY = "A1B2C3D4E5F6..."
BASE_URL = "https://soporte.miempresa.com"

def crear_ticket(nombre, email, asunto, mensaje, topic_id=1):
    payload = {
        "name": nombre,
        "email": email,
        "subject": asunto,
        "message": f"data:text/html,{mensaje}",
        "topicId": topic_id,
        "source": "API"
    }
    headers = {
        "X-API-Key": API_KEY,
        "Content-Type": "application/json"
    }
    resp = requests.post(
        f"{BASE_URL}/api/tickets.json",
        headers=headers,
        data=json.dumps(payload)
    )
    return resp.json()

ticket = crear_ticket(
    nombre="Carlos López",
    email="carlos@empresa.com",
    asunto="Falla en reporte mensual",
    mensaje="El reporte de ventas de febrero no genera el PDF.",
    topic_id=5
)
print(ticket)
```

#### PHP — crear ticket

```php
<?php
$apiKey = 'A1B2C3D4E5F6...';
$url    = 'https://soporte.miempresa.com/api/tickets.json';

$data = json_encode([
    'name'     => 'Pedro Soto',
    'email'    => 'pedro@cliente.com',
    'subject'  => 'Consulta de facturación',
    'message'  => 'data:text/html,Necesito una copia de mi factura del mes pasado.',
    'topicId'  => 2,
]);

$ch = curl_init($url);
curl_setopt_array($ch, [
    CURLOPT_POST           => true,
    CURLOPT_POSTFIELDS     => $data,
    CURLOPT_RETURNTRANSFER => true,
    CURLOPT_HTTPHEADER     => [
        'X-API-Key: ' . $apiKey,
        'Content-Type: application/json',
    ],
]);
$response = curl_exec($ch);
curl_close($ch);
echo $response;
```

---

## 15. Configuración avanzada

### 15.1 ost-config.php

Archivo de configuración principal ubicado en `include/ost-config.php`:

```php
<?php
define('OSTTICKET_VERSION','1.18.1');

// Base URL del sistema
define('ROOT_PATH', '/var/www/html/osticket/');

# Base de datos
define('DBHOST','localhost');
define('DBNAME','osticket');
define('DBUSER','osticket');
define('DBPASS','password_seguro');
define('DBPREFIX','ost_');    // prefijo de tablas
define('DBTYPE','mysql');

# Configuraciones de seguridad
define('SECRET_SALT','tu_salt_aleatorio_aqui');
define('ADMIN_EMAIL','admin@empresa.com');

# Configuración avanzada
define('OSTICKET_CRON','5');        // minutos entre ejecuciones de cron
define('TICKET_LOCK','30');         // segundos de bloqueo de ticket
define('DEFAULT_TIMEZONE','America/Santiago');
define('DEFAULT_LANG_CODE','es_ES');

# Logs
define('LOG_LEVEL', 1);             // 0=off, 1=errors, 2=debug
define('LOG_GRACEPERIOD', 3);       // días de retención de logs
```

### 15.2 Cron jobs

El cron job es esencial para el procesamiento automático de emails y alarmas de SLA.

#### Linux/Unix

```bash
# Editar crontab del usuario web
sudo crontab -u www-data -e

# Agregar línea (ejecuta cada 5 minutos)
*/5 * * * * php /var/www/html/osticket/api/cron.php
```

#### Windows (Task Scheduler)

```powershell
# Crear tarea programada
$action = New-ScheduledTaskAction -Execute "php" -Argument "C:\inetpub\wwwroot\osticket\api\cron.php"
$trigger = New-ScheduledTaskTrigger -RepetitionInterval (New-TimeSpan -Minutes 5) -Once -At (Get-Date)
Register-ScheduledTask -Action $action -Trigger $trigger -TaskName "osTicket Cron" -RunLevel Highest
```

#### Docker

```yaml
# Agregar servicio cron al docker-compose.yml
  cron:
    image: osticket/osticket:latest
    depends_on:
      - db
    environment:
      *env_vars
    command: >
      sh -c "while true; do php /var/www/html/api/cron.php; sleep 300; done"
```

### 15.3 LDAP / Active Directory

Requiere el plugin `auth-ldap` instalado y activado.

```
Admin Panel > Manage > Plugins > LDAP Authentication > Configure

LDAP Server:     ldap://dc.miempresa.local
Port:            389 (LDAP) / 636 (LDAPS)
Domain:          miempresa.local
Search Base DN:  OU=Usuarios,DC=miempresa,DC=local
Bind DN:         CN=osticket,OU=Servicios,DC=miempresa,DC=local
Bind Password:   *****

Atributos de mapeo:
  Email:    mail
  Name:     displayName
  Phone:    telephoneNumber

Auto-create users: [✓]
Default Role:      Agente
```

### 15.4 OAuth2 / SSO

Requiere el plugin `auth-oauth2`.

#### Configuración con Google

```
Admin Panel > Manage > Plugins > OAuth2 Authentication > Configure

Provider:          Google
Client ID:         xxxx.apps.googleusercontent.com
Client Secret:     *****
Callback URL:      https://soporte.miempresa.com/api/auth/oauth2
Scopes:            email profile

Hosted Domain:     miempresa.com  (solo emails de tu dominio)
```

#### Configuración con Azure AD / Microsoft

```
Provider:          Microsoft
Tenant ID:         xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
Client ID:         xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
Client Secret:     *****
Callback URL:      https://soporte.miempresa.com/api/auth/oauth2
```

---

## 16. Seguridad

### Buenas prácticas recomendadas

#### Post-instalación

```bash
# 1. Eliminar directorio de setup (OBLIGATORIO)
rm -rf /var/www/html/osticket/setup/

# 2. Proteger ost-config.php contra escritura
chmod 0644 /var/www/html/osticket/include/ost-config.php

# 3. Restringir acceso al directorio include/
# En .htaccess o httpd.conf:
```

```apache
# Bloquear acceso web a /include/
<Directory /var/www/html/osticket/include/>
    Order deny,allow
    Deny from all
</Directory>
```

```nginx
# Nginx equivalente
location ~ ^/include/ {
    deny all;
    return 403;
}
```

#### Configuración de seguridad en Admin Panel

```
Admin Panel > Settings > Security

Max Login Attempts:     5
Lockout Period:         30 minutes
Enable CSRF Token:      Yes
Force HTTPS:            Yes (si tienes SSL)
Session Lifetime:       30 minutes
Password Complexity:    Strict (mayúscula, número, especial)
Password Expiry:        90 days
```

#### HTTPS / SSL con Let's Encrypt

```bash
# Instalar Certbot
sudo apt install certbot python3-certbot-apache -y

# Obtener certificado
sudo certbot --apache -d soporte.miempresa.com

# Renovación automática
sudo crontab -e
# Agregar:
0 12 * * * certbot renew --quiet
```

#### Firewall básico

```bash
# UFW (Ubuntu)
sudo ufw allow 22/tcp    # SSH
sudo ufw allow 80/tcp    # HTTP
sudo ufw allow 443/tcp   # HTTPS
sudo ufw deny 3306/tcp   # Bloquear acceso externo a MySQL
sudo ufw enable
```

#### Auditoría (plugin)

Con el plugin `audit` activado, navegue a:
```
Admin Panel > Manage > Audit Log
```
Registra: logins, cambios de configuración, creación/modificación de agentes.

---

## 17. Backups y restauración

### Backup de base de datos

```bash
#!/bin/bash
# /opt/backup-osticket.sh

DATE=$(date +%Y%m%d_%H%M%S)
BACKUP_DIR="/opt/backups/osticket"
DB_NAME="osticket"
DB_USER="osticket"
DB_PASS="password"

mkdir -p "$BACKUP_DIR"

# Backup BD
mysqldump -u "$DB_USER" -p"$DB_PASS" "$DB_NAME" \
  | gzip > "$BACKUP_DIR/db_${DATE}.sql.gz"

# Backup archivos (adjuntos y config)
tar -czf "$BACKUP_DIR/files_${DATE}.tar.gz" \
  /var/www/html/osticket/upload/ \
  /var/www/html/osticket/include/ost-config.php

# Eliminar backups más viejos de 30 días
find "$BACKUP_DIR" -mtime +30 -delete

echo "Backup completado: $DATE"
```

```bash
# Programar en cron (diario a las 2am)
sudo crontab -e
0 2 * * * /opt/backup-osticket.sh >> /var/log/osticket-backup.log 2>&1
```

### Restauración

```bash
# 1. Restaurar base de datos
gunzip -c /opt/backups/osticket/db_20260101_020000.sql.gz | mysql -u osticket -p osticket

# 2. Restaurar archivos
tar -xzf /opt/backups/osticket/files_20260101_020000.tar.gz -C /

# 3. Verificar permisos
chown -R www-data:www-data /var/www/html/osticket/upload/
chmod 0644 /var/www/html/osticket/include/ost-config.php
```

---

## 18. Actualización de versión

### Proceso de actualización

> **⚠️ SIEMPRE hacer backup completo antes de actualizar**

```bash
# 1. Backup completo
mysqldump -u osticket -p osticket > /opt/backups/pre-upgrade.sql
tar -czf /opt/backups/pre-upgrade-files.tar.gz /var/www/html/osticket/

# 2. Descargar nueva versión
cd /tmp
wget https://github.com/osTicket/osTicket/releases/download/v1.18.1/osTicket-v1.18.1.zip
unzip osTicket-v1.18.1.zip -d osticket-new

# 3. Modo mantenimiento (crear archivo)
touch /var/www/html/osticket/setup/upgrade.php
# (realmente en osTicket el upgrade se hace via web en la URL /setup/upgrade.php)

# 4. Reemplazar archivos (excepto ost-config.php y upload/)
rsync -av --exclude='include/ost-config.php' \
          --exclude='upload/' \
          --exclude='plugins/' \
          osticket-new/ /var/www/html/osticket/

# 5. Ejecutar migración de base de datos via web
# Navegar a: https://soporte.miempresa.com/setup/upgrade.php

# 6. Eliminar setup tras actualizar
rm -rf /var/www/html/osticket/setup/

# 7. Limpiar caché
rm -rf /var/www/html/osticket/bootstrap/cache/*
```

---

## 19. Resolución de problemas comunes

### Error: "Cron has not been setup" / tickets de email no llegan

```bash
# Verificar que el cron esté ejecutándose
crontab -u www-data -l

# Ejecutar manualmente para ver errores
sudo -u www-data php /var/www/html/osticket/api/cron.php

# Ver logs del proceso
tail -f /var/log/apache2/osticket_error.log
```

### Error: "Unable to send email"

```
1. Verificar credenciales SMTP en Admin > Emails > Settings
2. Verificar que el puerto SMTP no esté bloqueado: 
   telnet mail.miempresa.com 587
3. Revisar logs de PHP:
   tail -f /var/log/php_errors.log
4. Probar con un email de prueba:
   Admin > Emails > [email] > Test Mail Settings
```

### Tickets duplicados desde email

```
Causa: El cron se ejecuta muy seguido o el IMAP no archiva correctamente

Solución:
1. Configurar "Archive Folder" en la configuración del email
2. Verificar que el usuario IMAP tiene permisos de mover mensajes
3. Reducir frecuencia del cron si hay solapamiento
```

### Portal lento / tiempo de carga alto

```bash
# Habilitar caché de PHP (OPcache)
# En php.ini:
opcache.enable=1
opcache.memory_consumption=128
opcache.max_accelerated_files=10000

# Revisar queries lentas en MySQL
SET GLOBAL slow_query_log = 'ON';
SET GLOBAL long_query_time = 1;

# Optimizar tablas de BD
mysqlcheck -u osticket -p --optimize osticket
```

### Error 500 al acceder

```bash
# 1. Verificar permisos
ls -la /var/www/html/osticket/include/ost-config.php
# Debe ser -rw-r--r-- (644)

# 2. Verificar logs de Apache
tail -50 /var/log/apache2/osticket_error.log

# 3. Verificar sintaxis PHP
php -l /var/www/html/osticket/include/ost-config.php

# 4. Verificar que .htaccess tenga AllowOverride All en Apache
```

### Adjuntos no se guardan

```bash
# Verificar permisos del directorio upload/
ls -la /var/www/html/osticket/upload/
# Debe ser drwxrwxr-x www-data:www-data

# Corregir permisos
chown -R www-data:www-data /var/www/html/osticket/upload/
chmod -R 775 /var/www/html/osticket/upload/

# Verificar límite de tamaño en php.ini
upload_max_filesize = 20M
post_max_size = 25M
```

### Resetear contraseña de administrador

```sql
-- Conectar a MySQL y ejecutar:
USE osticket;

-- Ver el staff admin
SELECT staff_id, username, email FROM ost_staff WHERE isadmin = 1;

-- Generar nuevo hash (ej: Admin1234!)
-- En PHP: echo password_hash('Admin1234!', PASSWORD_BCRYPT);
-- O via script PHP:
-- <?php echo password_hash('Admin1234!', PASSWORD_BCRYPT); ?>

UPDATE ost_staff 
SET passwd = '$2y$10$HASH_GENERADO_AQUI' 
WHERE username = 'admin';
```

---

## 20. Referencia de estructura de base de datos

Las tablas principales usan el prefijo configurado (`ost_` por defecto).

### Tablas principales

| Tabla | Descripción |
|---|---|
| `ost_ticket` | Tickets principales |
| `ost_ticket__cdata` | Campos dinámicos de tickets |
| `ost_ticket_thread` | Hilos de conversación |
| `ost_thread_entry` | Entradas individuales del hilo |
| `ost_users` | Usuarios finales |
| `ost_user_account` | Cuentas de usuario (login) |
| `ost_staff` | Agentes/staff |
| `ost_department` | Departamentos |
| `ost_groups` | Roles de agentes |
| `ost_sla` | Planes de SLA |
| `ost_priority` | Niveles de prioridad |
| `ost_help_topic` | Temas de ayuda |
| `ost_email` | Configuración de emails |
| `ost_filter` | Filtros de tickets |
| `ost_filter_action` | Acciones de filtros |
| `ost_canned_response` | Respuestas predefinidas |
| `ost_faq` | Artículos de FAQ/KB |
| `ost_attachment` | Meta-información de adjuntos |
| `ost_file` | Archivos almacenados |
| `ost_config` | Configuración del sistema |
| `ost_api_key` | Claves de API |

### Query útil: tickets abiertos por departamento

```sql
SELECT 
    d.name AS departamento,
    COUNT(t.ticket_id) AS tickets_abiertos,
    AVG(TIMESTAMPDIFF(HOUR, t.created, NOW())) AS horas_promedio
FROM ost_ticket t
JOIN ost_department d ON t.dept_id = d.id
WHERE t.status_id = 1  -- 1=Open
GROUP BY d.id, d.name
ORDER BY tickets_abiertos DESC;
```

### Query útil: rendimiento de agentes (últimos 30 días)

```sql
SELECT 
    CONCAT(s.firstname, ' ', s.lastname) AS agente,
    COUNT(te.id) AS respuestas,
    COUNT(DISTINCT te.thread_id) AS tickets_respondidos
FROM ost_thread_entry te
JOIN ost_staff s ON te.staff_id = s.staff_id
WHERE te.type = 'R'  -- R=Reply
  AND te.created >= DATE_SUB(NOW(), INTERVAL 30 DAY)
GROUP BY s.staff_id
ORDER BY respuestas DESC;
```

---

## Apéndice A — Glosario

| Término | Definición |
|---|---|
| **Ticket** | Solicitud de soporte creada por un usuario |
| **Staff / Agente** | Persona que gestiona y responde tickets |
| **Department** | Unidad organizativa que recibe tickets de un área |
| **Team** | Grupo de agentes de distintos departamentos |
| **Help Topic** | Categoría visible para el usuario al abrir un ticket |
| **SLA** | Acuerdo de nivel de servicio (tiempo máximo de respuesta) |
| **Canned Response** | Plantilla de respuesta predefinida para agentes |
| **Ticket Filter** | Regla automática que procesa tickets al crearse |
| **Override** | Opción para ignorar settings globales en objeto específico |
| **Thread** | Conversación completa de un ticket |
| **Internal Note** | Comentario visible solo para staff |
| **Knowledgebase** | Base de conocimiento pública de artículos de ayuda |
| **Cron** | Proceso programado para procesamiento automático |
| **API Key** | Clave de autenticación para la API REST |

## Apéndice B — Recursos oficiales

| Recurso | URL |
|---|---|
| Sitio oficial | https://osticket.com |
| Repo GitHub | https://github.com/osTicket/osTicket |
| Foros de soporte | https://forum.osticket.com |
| Documentación oficial | https://docs.osticket.com |
| Plugins oficiales | https://github.com/osTicket/osTicket-plugins |
| Demo en línea | https://www.osticket.com/demo |

---

*Documento generado el 11 de marzo de 2026. Para la versión más actualizada de la documentación oficial, visitar https://docs.osticket.com*

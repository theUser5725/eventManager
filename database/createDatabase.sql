-- Tabla: CategoriasEventos
CREATE TABLE CategoriasEventos (
    idCatEvento INTEGER PRIMARY KEY,
    nombre TEXT NOT NULL
);

-- Tabla: Eventos
CREATE TABLE Eventos (
    idEvento INTEGER PRIMARY KEY,
    idCatEvento INTEGER NOT NULL,
    nombre TEXT NOT NULL,
    totalHoras INTEGER,
    FOREIGN KEY (idCatEvento) REFERENCES CategoriasEventos(idCatEvento)
);

-- Tabla: Lugares
CREATE TABLE Lugares (
    idLugar INTEGER PRIMARY KEY,
    nombre TEXT NOT NULL,
    capacidad INTEGER
);

-- Tabla: Reuniones
CREATE TABLE Reuniones (
    idReunion INTEGER PRIMARY KEY,
    idEvento INTEGER NOT NULL,
    idLugar INTEGER NOT NULL,
    horario TEXT NOT NULL,
    FOREIGN KEY (idEvento) REFERENCES Eventos(idEvento),
    FOREIGN KEY (idLugar) REFERENCES Lugares(idLugar)
);

-- Tabla: Participantes
CREATE TABLE Participantes (
    idParticipante INTEGER PRIMARY KEY,
    nombre TEXT NOT NULL,
    apellido TEXT NOT NULL,
    mail TEXT NOT NULL UNIQUE,
    dni TEXT NOT NULL UNIQUE,
    contraseña TEXT NOT NULL
);

-- Tabla: Inscripciones
CREATE TABLE Inscripciones (
    idEvento INTEGER,
    idParticipante INTEGER,
    PRIMARY KEY (idEvento, idParticipante),
    FOREIGN KEY (idEvento) REFERENCES Eventos(idEvento),
    FOREIGN KEY (idParticipante) REFERENCES Participantes(idParticipante)
);

-- Tabla: Asistencias
CREATE TABLE Asistencias (
    idEvento INTEGER,
    idParticipante INTEGER,
    horariosAsistido TEXT,
    PRIMARY KEY (idEvento, idParticipante),
    FOREIGN KEY (idEvento) REFERENCES Eventos(idEvento),
    FOREIGN KEY (idParticipante) REFERENCES Participantes(idParticipante)
);

-- Tabla: CategoriasDirectivos
CREATE TABLE CategoriasDirectivos (
    idCatDirectiva INTEGER PRIMARY KEY,
    nombre TEXT NOT NULL
);

-- Tabla: Directivos
CREATE TABLE Directivos (
    idReunion INTEGER,
    idParticipante INTEGER,
    idCatDirectiva INTEGER NOT NULL,
    PRIMARY KEY (idReunion, idParticipante),
    FOREIGN KEY (idReunion) REFERENCES Reuniones(idReunion),
    FOREIGN KEY (idParticipante) REFERENCES Participantes(idParticipante),
    FOREIGN KEY (idCatDirectiva) REFERENCES CategoriasDirectivos(idCatDirectiva)
);
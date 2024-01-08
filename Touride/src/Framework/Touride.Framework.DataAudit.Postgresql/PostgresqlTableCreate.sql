-- Table: public.auditlogs

-- DROP TABLE public.auditlogs;

CREATE TABLE public.auditlogs
(
    id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    eventtime date NOT NULL,
    eventtype character varying(50) COLLATE pg_catalog."default" NOT NULL,
    pk1 bigint,
    pk2 bigint,
    data text COLLATE pg_catalog."default",
    database text COLLATE pg_catalog."default",
    schema character varying(100) COLLATE pg_catalog."default",
    tablo character varying(100) COLLATE pg_catalog."default",
    kullanici character varying(100) COLLATE pg_catalog."default",
    CONSTRAINT auditlogs_pkey PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE public.auditlogs
    OWNER to postgres;
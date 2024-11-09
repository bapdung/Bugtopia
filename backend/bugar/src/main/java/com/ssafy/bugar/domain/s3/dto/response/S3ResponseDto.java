package com.ssafy.bugar.domain.s3.dto.response;

public record S3ResponseDto(String url) {
	public static S3ResponseDto from(String url) {
		return new S3ResponseDto(url);
	}
}

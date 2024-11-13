package com.ssafy.bugar.domain.s3.controller;

import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.ssafy.bugar.domain.s3.service.S3Service;
import com.ssafy.bugar.domain.s3.dto.request.S3RequestDto;
import com.ssafy.bugar.domain.s3.dto.response.S3ResponseDto;

import lombok.RequiredArgsConstructor;

@CrossOrigin
@RestController
@RequestMapping("/api/files")
@RequiredArgsConstructor
public class S3Controller {
	private final S3Service s3Service;

	@PostMapping("/upload/{fileName}")
	S3ResponseDto createPresignedUrl(@RequestBody S3RequestDto s3RequestDto, @PathVariable String fileName) {
		return s3Service.createPresignedUrl(s3RequestDto, fileName);
	}
}